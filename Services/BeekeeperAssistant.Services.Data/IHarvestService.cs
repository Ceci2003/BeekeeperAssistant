namespace BeekeeperAssistant.Services.Data
{
    using BeekeeperAssistant.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IHarvestService
    {
        Task<int> CreateUserHarvestAsync(
            string userId,
            int beehiveId,
            string harvestName,
            DateTime dateOfHarves,
            string product,
            HoneyType honeyType,
            string note,
            int amount);

        Task<int> EditHarvestAsync(
            int harvestId,
            string harvestName,
            DateTime dateOfHarves,
            string product,
            HoneyType honeyType,
            string note,
            int amount);

        Task DeleteHarvestAsync(int harvestId);

        T GetHarvestById<T>(int harvestId);

        IEnumerable<T> GetAllUserHarvests<T>(string userId);
    }
}
