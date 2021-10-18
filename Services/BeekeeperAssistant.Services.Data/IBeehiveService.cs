namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface IBeehiveService
    {
        Task<int> CreateUserBeehiveAsync(
            string userId,
            int number,
            BeehiveSystem beehiveSystem,
            BeehiveType beehiveType,
            DateTime dateTime,
            int apiaryId,
            BeehivePower beehivePower,
            bool hasDevice,
            bool hasPolenCatcher,
            bool hasPropolisCatcher,
            bool isItMovable);

        T GetBeehiveById<T>(int beehiveId);

        IEnumerable<T> GetAllUserBeehives<T>(string userId, int? take = null, int skip = 0);

        Task<string> DeleteBeehiveByIdAsync(int beehiveId);

        Task<int> EditUserBeehiveAsync(
            int beehiveId,
            int number,
            BeehiveSystem beehiveSystem,
            BeehiveType beehiveType,
            DateTime dateTime,
            int apiaryId,
            BeehivePower beehivePower,
            bool hasDevice,
            bool hasPolenCatcher,
            bool hasPropolisCatcher,
            bool isItMovable);

        IEnumerable<T> GetBeehivesByApiaryId<T>(int apiaryId, int? take = null, int skip = 0);

        int GetAllBeehivesCountByApiaryId(int apiaryId);

        T GetBeehiveByNumber<T>(int beehiveNumber, string apiaryNumber);

        int GetAllUserBeehivesCount(string userId);
    }
}
