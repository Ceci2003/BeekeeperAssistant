namespace BeekeeperAssistant.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    internal class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedUserAsync(userManager, "i_admin@beekeeperassistant.com", GlobalConstants.AdministratorRoleName);
            await SeedUserAsync(userManager, "c_admin@beekeeperassistant.com", GlobalConstants.AdministratorRoleName);
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, string username, string roleName)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                var appUser = new ApplicationUser();
                appUser.UserName = username;
                appUser.Email = username;

                IdentityResult result = new IdentityResult();

                if (roleName == GlobalConstants.AdministratorRoleName)
                {
                    result = userManager.CreateAsync(appUser, "admin123").Result;
                }

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(appUser, roleName).Wait();
                }
            }
        }
    }
}
