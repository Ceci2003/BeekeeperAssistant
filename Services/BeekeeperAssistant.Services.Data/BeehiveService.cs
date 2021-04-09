using BeekeeperAssistant.Data.Common.Repositories;
using BeekeeperAssistant.Data.Models;
using System;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Services.Data
{
    public class BeehiveService : IBeehiveService
    {
        private readonly IDeletableEntityRepository<Beehive> beehiveRepository;

        public BeehiveService(IDeletableEntityRepository<Beehive> beehiveRepository)
        {
            this.beehiveRepository = beehiveRepository;
        }

        public async Task<int> CreateUserBeehiveAsync(string userId, int number, BeehiveSystem beehiveSystem, BeehiveType beehiveType, DateTime dateTime, int apiaryId, BeehivePower beehivePower, bool hasDevice, bool hasPolenCatcher, bool hasPropolisCatcher)
        {
            var beehive = new Beehive
            {
                CreatorId = userId,
                Number = number,
                BeehiveSystem = beehiveSystem,
                BeehiveType = beehiveType,
                BeehivePower = beehivePower,
                Date = dateTime,
                ApiaryId = apiaryId,
                HasDevice = hasDevice,
                HasPolenCatcher = hasPolenCatcher,
                HasPropolisCatcher = hasPropolisCatcher,
            };

            await this.beehiveRepository.AddAsync(beehive);
            await this.beehiveRepository.SaveChangesAsync();

            return beehive.Id;
        }
    }
}
