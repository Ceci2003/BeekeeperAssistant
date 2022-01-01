using System.Threading.Tasks;

namespace BeekeeperAssistant.Services.Data
{
    public interface IBeehiveDiaryService
    {
        Task<int> CreateAsync(int beehiveId, string content, string modifiedBy);

        T GetBeehiveDiaryByBeehiveId<T>(int beehiveId);

        Task<int> SaveAsync(int beehiveId, string content, string modifiedById);
    }
}
