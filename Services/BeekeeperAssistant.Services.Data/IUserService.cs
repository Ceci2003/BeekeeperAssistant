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

        IEnumerable<T> GetAllUsersWithDeleted<T>();

        Task DeleteAsync(ApplicationUser user);

        Task UndeleteAsync(ApplicationUser user);

        IEnumerable<T> GetAllUsersInRole<T>(string role);

        ApplicationUser GetUserByIdWithUndeleted(string userId);
    }
}
