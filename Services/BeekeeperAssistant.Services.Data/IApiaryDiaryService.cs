namespace BeekeeperAssistant.Services.Data
{
    using System.Threading.Tasks;

    public interface IApiaryDiaryService
    {
        Task<int> CreateAsync(int apiaryId, string content, string modifiedBy);

        T GetApiaryDiaryByApiaryId<T>(int apiaryId);

        Task<int> SaveAsync(int apiaryId, string content, string modifiedBy);
    }
}
