namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface IUserService
    {
        int AllUsersCount();

        Task<int> AllAdministratorsCountAsync();

        IEnumerable<T> GetAllUsers<T>();

        IEnumerable<T> GetAllUsersWithDeleted<T>(int? take = null, int skip = 0);

        int GetAllUsersWithDeletedCount();

        Task DeleteAsync(ApplicationUser user);

        Task UndeleteAsync(ApplicationUser user);

        IEnumerable<T> GetAllUsersInRole<T>(string role, int? take = null, int skip = 0);

        IEnumerable<T> GetAllUsersInRoleWithDeleted<T>(string role, int? take = null, int skip = 0);

        int GetAllUsersInRoleCount(string role);

        int GetAllUsersInRoleWithDeletedCount(string role);

        ApplicationUser GetUserByIdWithUndeleted(string userId);

        ApplicationUser GetAllUserInfoAsync(string userId);
    }
}
