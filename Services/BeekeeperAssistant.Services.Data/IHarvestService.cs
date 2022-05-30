﻿namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Data.Filters.Models;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data.Models;
    using BeekeeperAssistant.Web.ViewModels.Harvests;

    public interface IHarvestService
    {
        Task<int> CreateUserHarvestAsync(
            string ownerId,
            string creatorId,
            CreateHarvestInputModel inputModel,
            List<int> beehiveIds);

        Task<int> EditHarvestAsync(
            int harvestId,
            EditHarvestInputModel inputModel);

        Task DeleteHarvestAsync(int harvestId);

        T GetHarvestById<T>(int harvestId);

        int GetAllUserHarvestsForLastYearCount(string userId);

        int GetAllBeehiveHarvestsCountByBeehiveId(int beehiveId);

        IEnumerable<T> GetAllUserHarvests<T>(string userId);

        IEnumerable<T> GetAllBeehiveHarvests<T>(int beehiveId, int? take = null, int skip = 0, FilterModel filterModel = null);
    }
}
