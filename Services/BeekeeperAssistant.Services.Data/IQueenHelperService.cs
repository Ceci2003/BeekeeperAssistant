namespace BeekeeperAssistant.Services.Data
{
    using BeekeeperAssistant.Data.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IQueenHelperService
    {
        Task Edit(string userId, int queenId, Access access);

        T GetQueenHelper<T>(string userId, int queenId);

        IEnumerable<T> GetAllQueenByQueenId<T>(int queenId, int? take = null, int skip = 0);
    }
}
