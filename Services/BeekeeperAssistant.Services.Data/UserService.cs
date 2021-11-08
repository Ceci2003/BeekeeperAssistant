namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using Microsoft.AspNetCore.Identity;

    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<ApplicationRole> roleRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<ApplicationRole> roleRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.userManager = userManager;
        }

        public async Task<int> AdministratorsCount()
        {
            var result = await this.userManager.GetUsersInRoleAsync(GlobalConstants.AdministratorRoleName);
            return result.Count();
        }

        public int Count()
            => this.userRepository.All().Count();
    }
}
