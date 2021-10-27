namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface IApiaryHelperService
    {
        Task Add(string userId, int apiaryId);

        Task Delete(string userId, int apiaryId);

        Task Edit(string userId, int apiaryId, Access access);

        T GetApiaryHelper<T>(string userId, int apiaryId);

        bool IsApiaryHelper(string userId, int apiaryId);

        IEnumerable<T> GetUserHelperApiaries<T>(string userId, int? take = null, int skip = 0);

        IEnumerable<T> GetAllApiaryHelpersByApiaryId<T>(int apiaryId, int? take = null, int skip = 0);

        int GetUserHelperApiariesCount(string userId);

        Access GetUserApiaryAccess(string userId, int apiaryId);

    }
}
