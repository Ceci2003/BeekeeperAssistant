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
    using BeekeeperAssistant.Services.Mapping;

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

        public async Task<int> AllAdministratorsCountAsync()
        {
            var result = await this.userManager.GetUsersInRoleAsync(GlobalConstants.AdministratorRoleName);
            return result.Count();
        }

        public int AllUsersCount()
            => this.userRepository.All().Count();

        public async Task DeleteAsync(ApplicationUser user)
        {
            this.userRepository.Delete(user);
            await this.userRepository.SaveChangesAsync();
        }

        // TODO: Add pagination
        public IEnumerable<T> GetAllUsers<T>()
        => this.userRepository
            .All()
            .To<T>()
            .ToList();

        public IEnumerable<T> GetAllUsersInRole<T>(string role)
        {
            var roleId = this.roleRepository.All()
                .FirstOrDefault(x => x.NormalizedName == role.ToUpper())
                .Id;

            var allUsers = this.userManager.Users
                .Where(x => x.Roles
                    .Any(y => y.RoleId == roleId))
                .To<T>()
                .ToList();

            return allUsers;
        }

        public IEnumerable<T> GetAllUsersWithDeleted<T>()
            => this.userRepository
                .AllWithDeleted()
                .To<T>()
                .ToList();

        public ApplicationUser GetUserByIdWithUndeleted(string userId)
            => this.userRepository
                .AllWithDeleted()
                .FirstOrDefault(u => u.Id == userId);

        public async Task UndeleteAsync(ApplicationUser user)
        {
            user.IsDeleted = false;
            user.DeletedOn = null;

            this.userRepository.Update(user);
            await this.userRepository.SaveChangesAsync();
        }
    }
}
