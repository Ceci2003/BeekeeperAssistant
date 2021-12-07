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
    using BeekeeperAssistant.Services.Mapping;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<T> GetAllUsersInRole<T>(string role, int? take = null, int skip = 0)
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

        public int GetAllUsersInRoleCount(string role)
        {
            var roleId = this.roleRepository.All()
                .FirstOrDefault(x => x.NormalizedName == role.ToUpper())
                .Id;

            var count = this.userManager.Users
                .Where(x => x.Roles
                .Any(y => y.RoleId == roleId))
                .Count();

            return count;
        }

        public int GetAllUsersInRoleWithDeletedCount(string role)
        {
            var roleId = this.roleRepository.All()
                .FirstOrDefault(x => x.NormalizedName == role.ToUpper())
                .Id;

            var count = this.userManager.Users
                .IgnoreQueryFilters()
                .Where(x => x.Roles
                .Any(y => y.RoleId == roleId))
                .Count();

            return count;
        }

        public IEnumerable<T> GetAllUsersInRoleWithDeleted<T>(string role, int? take = null, int skip = 0)
        {
            var roleId = this.roleRepository.All()
                .FirstOrDefault(x => x.NormalizedName == role.ToUpper())
                .Id;

            var query = this.userManager.Users
                .IgnoreQueryFilters()
                .Where(x => x.Roles
                .Any(y => y.RoleId == roleId))
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetAllUsersWithDeleted<T>(int? take = null, int skip = 0)
        {
            var query = this.userRepository
                .AllWithDeleted()
                .OrderByDescending(u => u.CreatedOn)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public int GetAllUsersWithDeletedCount()
         => this.userRepository
                .AllWithDeleted()
                .Count();

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
