using home_work_54_lab_work.Models;
using Microsoft.EntityFrameworkCore;
using home_work_54_lab_work.Models;
using home_work_54_lab_work.Services;
using Microsoft.AspNetCore.Identity;

class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
        builder.Services.AddControllersWithViews();
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
        app.Run();


        app.Run();
    }
    
}
