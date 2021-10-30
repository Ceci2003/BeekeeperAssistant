namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface IQueenHelperService
    {

        Task Edit(string userId, int queenId, Access access);

        T GetQueenHelper<T>(string userId, int queenId);

        IEnumerable<T> GetAllQueenByQueenId<T>(int queenId, int? take = null, int skip = 0);

        Access GetUserQueenAccess(string userId, int queenId);
    }
}
