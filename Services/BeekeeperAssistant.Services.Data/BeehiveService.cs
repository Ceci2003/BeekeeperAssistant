﻿namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

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

        public async Task<string> DeleteBeehiveByIdAsync(int beehiveId)
        {
            var beehive = this.beehiveRepository.All().Include(b => b.Apiary).FirstOrDefault(b => b.Id == beehiveId);

            this.beehiveRepository.Delete(beehive);
            await this.beehiveRepository.SaveChangesAsync();

            return beehive.Apiary.Number;
        }

        public T GetBeehiveById<T>(int beehiveId)
        {
            var beehive = this.beehiveRepository.All()
                .Where(b => b.Id == beehiveId)
                .To<T>()
                .FirstOrDefault();

            return beehive;
        }
    }
}
