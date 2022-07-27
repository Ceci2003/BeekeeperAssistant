namespace BeekeeperAssistant.Services.Data
{
    using System.Threading.Tasks;

    public interface IUserDiaryService
    {
        Task<int> CreateAsync(string content, string userId);

        T GetDiaryByUserId<T>(string userId);

        Task<int> SaveAsync(string userId, string content);

        bool HasDiary(string userId);
    }
}
