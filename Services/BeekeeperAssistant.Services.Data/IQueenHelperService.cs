namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface IQueenHelperService
    {
        Task EditAsync(string userId, int queenId, Access access);

        T GetQueenHelper<T>(string userId, int queenId);

        IEnumerable<T> GetAllQueenByQueenId<T>(int queenId, int? take = null, int skip = 0);

        Task<Access> GetUserQueenAccessAsync(string userId, int queenId);
    }
}
