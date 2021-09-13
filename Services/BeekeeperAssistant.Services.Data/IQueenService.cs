namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface IQueenService
    {
        Task<int> CreateUserQueenAsync(
            string userId,
            int beehiveId,
            DateTime fertilizationDate,
            DateTime givingDate,
            QueenType queenType,
            string origin,
            string hygenicHabits,
            string temperament,
            QueenColor queenColor,
            QueenBreed queenBreed);

        T GetQueenById<T>(int queenId);

        T GetQueenByBeehiveId<T>(int beehiveId);

        IEnumerable<T> GetAllUserQueens<T>(string userId, int? take = null, int skip = 0);

        Task<int> DeleteQueenAsync(int queenId);

        int GetAllUserQueensCount(string userId);

        Task<int> EditQueenAsync(
            int queenId,
            DateTime fertilizationDate,
            DateTime givingDate,
            QueenType queenType,
            string origin,
            string hygenicHabits,
            string temperament,
            QueenColor queenColor,
            QueenBreed queenBreed);
    }
}
