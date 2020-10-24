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

        T GetUserApiaryById<T>(int id, ApplicationUser user);

        Task DeleteById(int id, ApplicationUser user);

        Task EditUserApiaryById(int id, ApplicationUser user, EditApiaryInputModel editApiaryInputModel);

        Task AddUserApiary(ApplicationUser user, CreateApiaryInputModel inputModel);

        IEnumerable<T> GetAllUserApiaries<T>(string userId);

        Apiary GetUserApiaryByNumber(string apiNumber, ApplicationUser user);

        bool UserApiaryExists(string apiNumber, ApplicationUser user);
    }
}
