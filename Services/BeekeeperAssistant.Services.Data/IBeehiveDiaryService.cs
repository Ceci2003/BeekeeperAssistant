namespace BeekeeperAssistant.Services.Data
{
    using System.Threading.Tasks;

    public interface IBeehiveDiaryService
    {
        Task<int> CreateAsync(int beehiveId, string content, string modifiedBy);

        T GetBeehiveDiaryByBeehiveId<T>(int beehiveId);

        Task<int> SaveAsync(int beehiveId, string content, string modifiedById);
    }
}
