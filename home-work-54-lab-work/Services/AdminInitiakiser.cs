using home_work_54_lab_work.Models;
using Microsoft.AspNetCore.Identity;

namespace home_work_54_lab_work.Services;

public class AdminInitiakiser
{
    public static async Task SeedAdminAndRoleData(RoleManager<IdentityRole<int>> _roleManager, UserManager<User> _userManager)
    {
        string adminEmail = "admin@admin.admin";
        string adminPassword = "1qaz@WSX";
        var roles = new[] { "admin", "user" };
        foreach (var role in roles)
        {
            if (await _roleManager.FindByNameAsync(role) is null)
                await _roleManager.CreateAsync(new IdentityRole<int>(role));
        }

        if (await _userManager.FindByEmailAsync(adminEmail) == null)
        {
            User admin = new User() { Email = adminEmail, UserName = adminEmail };
            IdentityResult result = await _userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(admin, "admin");
        }
    }
}