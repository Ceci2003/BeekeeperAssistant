namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface IQueenService
    {
        Task<int> CreateUserQueenAsync(
            string userId,
            DateTime fertilizationDate,
            DateTime givingDate,
            QueenType queenType,
            string origin,
            string hygenicHabits,
            string temperament,
            QueenColor queenColor,
            QueenBreed queenBreed,
            int beehiveId);

        T GetQueenById<T>(int queenId);

        IEnumerable<T> GetAllUserQueens<T>(string userId);

        Task DeleteQueenAsync(int queenId);
    }
}
