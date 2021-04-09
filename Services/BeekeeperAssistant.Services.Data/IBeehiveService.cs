using BeekeeperAssistant.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Services.Data
{
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
            bool hasPropolisCatcher);
    }
}
