namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface IApiaryService
    {
        IEnumerable<T> GetAllUserApiaries<T>(string userId, int page = 1);

        string GetApiaryNumberByBeehiveId(int beehiveId);

        int GetUserApiaryIdByNumber(string userId, string apiaryNumber);

        T GetUserApiaryByNumber<T>(string userId, string number);

        T GetApiaryById<T>(int apiaryId);

        Task<string> CreateUserApiaryAsync(string userId, string number, string name, ApiaryType apiaryType, string address);

        Task DeleteApiaryByIdAsync(int apiaryId);

        Task<string> EditApiaryByIdAsync(int apiaryId, string number, string name, ApiaryType apiaryType, string address);

        public IEnumerable<KeyValuePair<int, string>> GetUserApiariesAsKeyValuePairs(string userId);
    }
}

// GetAllUserApiaries + pagination
// GetUserApiaryByNumber
// GetApairyById
// CreateUserApiary
// DeleteUserApiaryById / DeleteUserApiaryByNumber
// EditUserApiaryByNumber / EditUserApiaryById
