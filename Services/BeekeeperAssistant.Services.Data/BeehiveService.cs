namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
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

        public async Task<int> CreateUserBeehiveAsync(
            string userId,
            int number,
            BeehiveSystem beehiveSystem,
            BeehiveType beehiveType,
            DateTime dateTime,
            int apiaryId,
            BeehivePower beehivePower,
            bool hasDevice,
            bool hasPolenCatcher,
            bool hasPropolisCatcher)
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
            var beehive = this.beehiveRepository
                .All()
                .Include(b => b.Apiary)
                .FirstOrDefault(b => b.Id == beehiveId);

            this.beehiveRepository.Delete(beehive);
            await this.beehiveRepository.SaveChangesAsync();

            return beehive.Apiary.Number;
        }

        public async Task<int> EditUserBeehiveAsync(
            int beehiveId,
            int number,
            BeehiveSystem beehiveSystem,
            BeehiveType beehiveType,
            DateTime dateTime,
            int apiaryId,
            BeehivePower beehivePower,
            bool hasDevice,
            bool hasPolenCatcher,
            bool hasPropolisCatcher)
        {
            var beehive = this.beehiveRepository
                .All()
                .FirstOrDefault(b => b.Id == beehiveId);

            beehive.Number = number;
            beehive.BeehiveSystem = beehiveSystem;
            beehive.BeehiveType = beehiveType;
            beehive.BeehivePower = beehivePower;
            beehive.Date = dateTime;
            beehive.ApiaryId = apiaryId;
            beehive.HasDevice = hasDevice;
            beehive.HasPolenCatcher = hasPolenCatcher;
            beehive.HasPropolisCatcher = hasPropolisCatcher;

            await this.beehiveRepository.SaveChangesAsync();

            return beehive.Id;
        }

        public int GetAllBeehivesCountByApiaryId(int apiaryId) =>
            this.beehiveRepository
                .AllAsNoTracking()
                .Where(b => b.ApiaryId == apiaryId)
                .Count();

        public IEnumerable<T> GetAllUserBeehives<T>(string userId, int? take = null, int skip = 0)
        {
            var query = this.beehiveRepository
                .AllAsNoTracking()
                .Where(b => b.CreatorId == userId && b.Apiary.IsDeleted == false)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            var result = query.To<T>().ToList();
            return result;
        }

        public int GetAllUserBeehivesCount(string userId) =>
            this.beehiveRepository
                .AllAsNoTracking()
                .Where(b => b.CreatorId == userId && b.Apiary.IsDeleted == false)
                .Count();

        public IEnumerable<T> GetApiaryBeehivesById<T>(int apiaryId, int? take = null, int skip = 0)
        {
            var query = this.beehiveRepository.AllAsNoTracking()
                .OrderByDescending(b => b.CreatedOn)
                .Where(b => b.ApiaryId == apiaryId).Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetBeehiveById<T>(int beehiveId) =>
            this.beehiveRepository.AllAsNoTracking()
                .Where(b => b.Id == beehiveId)
                .To<T>()
                .FirstOrDefault();
    }
}
