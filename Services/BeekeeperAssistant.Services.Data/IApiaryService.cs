namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Data.Filters.Models;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data.Models;

    public interface IApiaryService
    {
        IEnumerable<T> GetAllUserApiaries<T>(string userId, int? take = null, int skip = 0, FilterModel filterModel = null);

        IEnumerable<T> GetAllUserMovableApiaries<T>(string userId, int? take = null, int skip = 0, FilterModel filterModel = null);

        string GetApiaryNumberByBeehiveId(int beehiveId);

        string GetApiaryNameByBeehiveId(int beehiveId);

        string GetApiaryNumberByApiaryId(int apiaryId);

        string GetApiaryNameByApiaryId(int apiaryId);

        bool IsApiaryCreator(string userId, int apiaryNumber);

        int GetApiaryIdByBeehiveId(int beehiveId);

        int GetApiaryIdByNumber(string apiaryNumber);

        T GetUserApiaryByNumber<T>(string userId, string number);

        T GetUserApiaryByBeehiveId<T>(int beehiveId);

        T GetApiaryById<T>(int apiaryId);

        T GetApiaryByNumber<T>(string apiaryNumber);

        Task BookmarkApiaryAsync(int apiaryId);

        Task<int> CreateUserApiaryAsync(
            string userId,
            string number,
            string name,
            ApiaryType apiaryType,
            string address,
            bool isRegistered,
            bool isClosed,
            DateTime? openingDate,
            DateTime? closingDate);

        Task DeleteApiaryByIdAsync(int apiaryId);

        Task<int> EditApiaryByIdAsync(
            int apiaryId,
            string number,
            string name,
            ApiaryType apiaryType,
            string address,
            bool isRegistered,
            bool isClosed,
            DateTime? openingDate,
            DateTime? closingDate);

        IEnumerable<KeyValuePair<int, string>> GetUserApiariesAsKeyValuePairs(string userId);

        IEnumerable<KeyValuePair<int, string>> GetUserApiariesWithoutTemporaryAsKeyValuePairs(string userId);

        int GetAllUserApiariesCount(string userId);

        int GetAllUserMovableApiariesCount(string userId);

        string GetApiaryOwnerIdByApiaryId(int apiaryId);

        IEnumerable<T> GetAllApiaries<T>();

        IEnumerable<T> GetAllApiariesWithDeleted<T>(int? take = null, int skip = 0, FilterModel filterModel = null);

        int GetAllApiariesWithDeletedCount();

        int AllApiariesCount();

        Task UndeleteAsync(int apiaryId);

        bool HasDiary(int apiaryId);

        Task UpdateMovableStatus(int apiaryId);
    }
}
