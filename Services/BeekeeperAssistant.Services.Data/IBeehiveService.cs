namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface IBeehiveService
    {
        Task<int> CreateBeehiveAsync(
            string ownerId,
            string creatorId,
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

        int GetLatestBeehiveNumber(int apiaryId);

        IEnumerable<T> GetAllUserBeehives<T>(string userId, int? take = null, int skip = 0, string orederBy = null);

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

        Task UpdateBeehiveApiary(int beehiveId, int apiaryId);

        Task UpdateBeehiveNumber(int beehiveId, int beehiveNumber);

        Task UpdateBeehiveNumberAndApiary(int beehiveId, int beehiveNumber, int apiaryId);

        IEnumerable<T> GetBeehivesByApiaryId<T>(int apiaryId, int? take = null, int skip = 0, string order = null);

        IEnumerable<T> GetBeehivesByApiaryIdWithoutInTemporary<T>(int apiaryId);

        int GetAllBeehivesCountByApiaryId(int apiaryId);

        T GetBeehiveByNumber<T>(int beehiveNumber, string apiaryNumber);

        int GetBeehiveIdByQueen(int queenId);

        T GetBeehiveByQueenId<T>(int queenId);

        int GetAllUserBeehivesCount(string userId);

        Task<int?> BookmarkBeehiveAsync(int id);

        int AllBeehivesCount();

        IEnumerable<T> GetAllBeehives<T>();

        IEnumerable<T> GetAllBeehivesWithDeleted<T>(int? take = null, int skip = 0, string orderBy = null);

        Task UndeleteAsync(int beehiveId);

        int GetAllBeehivesWithDeletedCount();

        int GetBeehiveNumberById(int id);

        int GetBeehiveIdByTreatmentId(int treatmentId);

        int GetBeehiveIdByHarvesId(int harvestId);

        bool HasDiary(int beehiveId);

        bool BeehiveNumberExistsInApiary(int beehiveNumber, int apiaryId);

        bool BeehiveExistsInApiary(int beehiveId, int apiaryId);
    }
}
