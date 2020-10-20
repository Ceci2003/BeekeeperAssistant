namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface IApiaryService
    {
        // GetAll
        IEnumerable<T> GetAllApiaries<T>();

        // GetByNumber
        T GetApiaryByNumber<T>(string number, ApplicationUser user);

        // GetById
        T GetApiaryById<T>(int id);

        // Delete
        void DeleteById(int id);

        // Edit
        void EditApiaryById();

        // Create
        Task AddApiary();

        // Get all user apiaries
        IEnumerable<T> GetAllUserApiaries<T>(string userId);

        int GetApiaryIdByNumber(string apiNumber, ApplicationUser user);

        bool ApiaryExists(string apiNumber, ApplicationUser user);
    }
}
