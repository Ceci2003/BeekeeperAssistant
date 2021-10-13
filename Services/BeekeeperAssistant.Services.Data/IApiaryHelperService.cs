namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IApiaryHelperService
    {
        Task Add(string userId, int apiaryId);

        bool IsAnApiaryHelper(string userId, int apiaryId);

        IEnumerable<T> GetUserHelperApiaries<T>(string userId, int? take = null, int skip = 0);

        int GetUserHelperApiariesCount(string userId);
    }
}
