namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface IBeehiveHelperService
    {
        Task Edit(string userId, int beehiveId, Access access);

        T GetBeehiveHelper<T>(string userId, int beehiveId);

        IEnumerable<T> GetAllBeehiveHelpersByBeehiveId<T>(int beehiveId, int? take = null, int skip = 0);
    }
}
