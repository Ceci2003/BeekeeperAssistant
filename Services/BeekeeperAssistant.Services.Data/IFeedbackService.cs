namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface IFeedbackService
    {
        Task CreateAsync(string userId, string title, string body, FeedbackType feedbackType);

        IEnumerable<T> GetAllFeedbackFeedbacks<T>(int? take = null, int skip = 0);

        IEnumerable<T> GetAllHelpFeedbacks<T>(int? take = null, int skip = 0);

        IEnumerable<T> GetAllFeedbacks<T>(int? take = null, int skip = 0);

        int GetAllHelpFeedbacksCount();

        int GetAllFeedbackFeedbacksCount();
    }
}
