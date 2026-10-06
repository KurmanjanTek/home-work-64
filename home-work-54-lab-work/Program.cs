using System.IO.Compression;
using home_work_54_lab_work.Models;
using Microsoft.EntityFrameworkCore;
using home_work_54_lab_work.Models;
using home_work_54_lab_work.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;

class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
        builder.Services.AddControllersWithViews(options =>
        {
            options.CacheProfiles.Add("Caching", new CacheProfile() { Duration = 300 });
            options.CacheProfiles.Add("NoCaching", new CacheProfile (){Location=ResponseCacheLocation.None, NoStore = true});
        });
        builder.Services.AddMemoryCache();
        builder.Services.AddResponseCompression(op=> op.EnableForHttps=true);
        builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.SmallestSize;
        });
        builder.Services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });
        string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<TaskContext>(options =>options.UseNpgsql(connection))
            .AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<TaskContext>();
        builder.Services.AddTransient<TaskItemService>();
        
        var app = builder.Build();

// Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapStaticAssets();

        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=TaskItem}/{action=Index}/{id?}")
            .WithStaticAssets();
        
        
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
            await AdminInitiakiser.SeedAdminAndRoleData(roleManager, userManager);
        }
        catch
        {
            Console.WriteLine("Error");
        }

        app.UseResponseCaching();
        app.Run();


       
    }
    
}
