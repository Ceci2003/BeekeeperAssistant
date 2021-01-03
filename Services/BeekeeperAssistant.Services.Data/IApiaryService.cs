namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;

    public interface IApiaryService
    {
        IEnumerable<T> GetAllApiaries<T>();

        T GetApiaryByNumber<T>(string number, ApplicationUser user);

        T GetApiaryById<T>(int id);

        Apiary GetApiaryById(int id);

        T GetUserApiaryById<T>(int id, ApplicationUser user);

        Apiary GetUserApiaryByNumber(string apiNumber, ApplicationUser user);

        Task DeleteById(int id, ApplicationUser user);

        Task EditUserApiaryById(int id, ApplicationUser user, EditApiaryInputModel editApiaryInputModel);

        Task AddUserApiary(ApplicationUser user, CreateApiaryInputModel inputModel);

        IEnumerable<T> GetAllUserApiaries<T>(string userId);

        bool UserApiaryExists(string apiNumber, ApplicationUser user);

        bool EditApiaryExist(string apiNumber, ApplicationUser user, int apiId);
    }
}
