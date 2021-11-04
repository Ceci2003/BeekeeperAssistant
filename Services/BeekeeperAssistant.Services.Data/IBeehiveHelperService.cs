﻿namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;

    public interface IBeehiveHelperService
    {
        Task Add(string userId, int beehiveId);

        Task Delete(string userId, int beehiveId);

        Task Edit(string userId, int beehiveId, Access access);

        T GetBeehiveHelper<T>(string userId, int beehiveId);

        bool IsBeehiveHelper(string userId, int beehiveId);

        IEnumerable<T> GetAllBeehiveHelpersByBeehiveId<T>(int beehiveId, int? take = null, int skip = 0);

        Access GetUserBeehiveAccess(string userId, int beehiveId);
    }
}
