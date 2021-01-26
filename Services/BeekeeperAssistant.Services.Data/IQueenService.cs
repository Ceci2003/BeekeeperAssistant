namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Web.ViewModels.Queens;

    public interface IQueenService
    {
        Task<int> CreateQueen(CreateQueenInputModel inputModel, int beehiveId, string currentUserId);

        T GetQueenById<T>(int id);

        IEnumerable<T> GetAllQueens<T>(int beehiveId, string currentUserId);

        IEnumerable<T> GetAllUserQueens<T>(string currentUserId);
    }
}
