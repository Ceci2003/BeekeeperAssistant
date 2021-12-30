namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Harvest;

    public class HarvestService : IHarvestService
    {
        private readonly IDeletableEntityRepository<Beehive> beehiveRepository;
        private readonly IDeletableEntityRepository<Harvest> harvestRepository;
        private readonly IRepository<HarvestedBeehive> harvestedBeehiveRepository;

        public HarvestService(
            IDeletableEntityRepository<Beehive> beehiveRepository,
            IDeletableEntityRepository<Harvest> harvestRepository,
            IRepository<HarvestedBeehive> harvestedBeehiveRepository)
        {
            this.beehiveRepository = beehiveRepository;
            this.harvestRepository = harvestRepository;
            this.harvestedBeehiveRepository = harvestedBeehiveRepository;
        }

        public async Task<int> CreateUserHarvestAsync(string ownerId, string creatorId, CreateHarvestInputModel inputModel, List<int> beehiveIds)
        {
            var harvest = new Harvest
            {
                OwnerId = ownerId,
                CreatorId = creatorId,
                HarvestName = inputModel.HarvestName,
                DateOfHarves = inputModel.DateOfHarves,
                Note = inputModel.Note,
                HarvestProductType = inputModel.HarvestProductType,
                HoneyType = inputModel.HoneyType,
                Quantity = Convert.ToDouble(inputModel.QuantityText),
                Unit = inputModel.Unit,
            };

            await this.harvestRepository.AddAsync(harvest);
            await this.harvestRepository.SaveChangesAsync();

            foreach (var id in beehiveIds)
            {
                var treatedBeehive = new HarvestedBeehive
                {
                    BeehiveId = id,
                    HarvestId = harvest.Id,
                };

                await this.harvestedBeehiveRepository.AddAsync(treatedBeehive);
                await this.harvestRepository.SaveChangesAsync();
            }

            return harvest.Id;
        }

        public async Task<int> EditHarvestAsync(int harvestId, EditHarvestInputModel inputModel)
        {
            var harvest = this.harvestRepository
                .All()
                .FirstOrDefault(h => h.Id == harvestId);

            harvest.HarvestName = inputModel.HarvestName;
            harvest.DateOfHarves = inputModel.DateOfHarves;
            harvest.Note = inputModel.Note;
            harvest.HarvestProductType = inputModel.HarvestProductType;
            harvest.HoneyType = inputModel.HoneyType;
            harvest.Quantity = Convert.ToDouble(inputModel.QuantityText);
            harvest.Unit = inputModel.Unit;

            await this.harvestRepository.SaveChangesAsync();

            return harvestId;
        }

        public async Task DeleteHarvestAsync(int harvestId)
        {
            var harvest = this.harvestRepository
                .All()
                .FirstOrDefault(h => h.Id == harvestId);

            this.harvestRepository.Delete(harvest);
            await this.harvestRepository.SaveChangesAsync();
        }

        public T GetHarvestById<T>(int harvestId) =>
            this.harvestRepository
            .All()
            .Where(h => h.Id == harvestId)
            .To<T>()
            .FirstOrDefault();

        public IEnumerable<T> GetAllUserHarvests<T>(string userId) =>
            this.harvestRepository
            .All()
            .OrderByDescending(hb => hb.DateOfHarves)
            .To<T>()
            .ToList();

        public int GetAllUserHarvestsForLastYearCount(string userId) =>
            this.harvestRepository
            .All()
            .SelectMany(x => x.HarvestedBeehives)
            .Where(h => !h.Beehive.IsDeleted && h.Harvest.CreatorId == userId)
            .Count();

        public IEnumerable<T> GetAllBeehiveHarvests<T>(int beehiveId, int? take = null, int skip = 0)
        {
            var query = this.harvestedBeehiveRepository
            .All()
            .Where(hb => hb.BeehiveId == beehiveId)
            .OrderByDescending(hb => hb.Harvest.DateOfHarves)
            .Select(hb => hb.Harvest)
            .Skip(skip);

            if (take.HasValue)
            {
                query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public int GetAllBeehiveHarvestsCountByBeehiveId(int beehiveId)
            => this.harvestedBeehiveRepository
            .All()
            .Where(hb => hb.BeehiveId == beehiveId && !hb.Harvest.IsDeleted)
            .Count();
    }
}
