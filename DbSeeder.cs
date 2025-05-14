using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp
{
    public class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Create roles if they do not exist
            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Create admin user if not exist
            var adminUser = await userManager.FindByEmailAsync("admin@mywebapp.com");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@mywebapp.com",
                    Email = "admin@mywebapp.com",
                    FullName = "Admin User"
                };
                await userManager.CreateAsync(adminUser, "AdminPassword123!");

                // Assign Admin role
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Create regular user if not exist
            var regularUser = await userManager.FindByEmailAsync("user@mywebapp.com");
            if (regularUser == null)
            {
                regularUser = new ApplicationUser
                {
                    UserName = "user@mywebapp.com",
                    Email = "user@mywebapp.com",
                    FullName = "Regular User"
                };
                await userManager.CreateAsync(regularUser, "UserPassword123!");

                // Assign User role
                await userManager.AddToRoleAsync(regularUser, "User");
            }
        }
    }
}