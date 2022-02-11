namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface IBeehiveMarkFlagService
    {
        Task<int> CreateBeehiveFlag(
            int beehiveId,
            string content,
            MarkFlagType flagType);

        Task<int> EditBeehiveFlag(
            int id,
            string content,
            MarkFlagType flagType);

        Task DeleteBeehiveFlag(int id);

        T GetBeehiveFlagByBeehiveId<T>(int beehiveId);

        T GetBeehiveFlagByFlagId<T>(int id);

        MarkFlagType? GetBeehiveFlagTypeByBeehiveId(int beehiveId);

        bool BeehiveHasFlag(int beehiveId);
    }
}
