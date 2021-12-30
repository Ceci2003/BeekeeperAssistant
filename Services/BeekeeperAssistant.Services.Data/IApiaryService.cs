namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface IApiaryService
    {
        IEnumerable<T> GetAllUserApiaries<T>(string userId, int? take = null, int skip = 0);

        string GetApiaryNumberByBeehiveId(int beehiveId);

        string GetApiaryNumberByApiaryId(int apiaryId);

        bool IsApiaryCreator(string userId, int apiaryNumber);

        int GetApiaryIdByBeehiveId(int beehiveId);

        int GetApiaryIdByNumber(string apiaryNumber);

        T GetUserApiaryByNumber<T>(string userId, string number);

        T GetUserApiaryByBeehiveId<T>(int beehiveId);

        T GetApiaryById<T>(int apiaryId);

        T GetApiaryByNumber<T>(string apiaryNumber);

        Task BookmarkApiaryAsync(int apiaryId);

        Task<string> CreateUserApiaryAsync(
            string userId,
            string number,
            string name,
            ApiaryType apiaryType,
            string address,
            bool IsRegistered);

        Task DeleteApiaryByIdAsync(int apiaryId);

        Task<string> EditApiaryByIdAsync(
            int apiaryId,
            string number,
            string name,
            ApiaryType apiaryType,
            string address,
            bool IsRegistered);

        IEnumerable<KeyValuePair<int, string>> GetUserApiariesAsKeyValuePairs(string userId);

        int GetAllUserApiariesCount(string userId);

        string GetApiaryOwnerIdByApiaryId(int apiaryId);

        IEnumerable<T> GetAllApiaries<T>();

        IEnumerable<T> GetAllApiariesWithDeleted<T>(int? take = null, int skip = 0);

        int GetAllApiariesWithDeletedCount();

        int AllApiariesCount();

        Task UndeleteAsync(int apiaryId);

        bool HasDiary(int apiaryId);
    }
}
