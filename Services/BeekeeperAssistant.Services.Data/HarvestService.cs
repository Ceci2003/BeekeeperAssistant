namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class HarvestService : IHarvestService
    {
        private readonly IDeletableEntityRepository<Harvest> harvestRepository;

        public HarvestService(
            IDeletableEntityRepository<Harvest> harvestRepository)
        {
            this.harvestRepository = harvestRepository;
        }

        public async Task<int> CreateUserHarvestAsync(
            string userId,
            int beehiveId,
            string harvestName,
            DateTime dateOfHarves,
            string product,
            HoneyType honeyType,
            string note,
            int amount)
        {
            var harvest = new Harvest
            {
                HarvestName = harvestName,
                DateOfHarves = dateOfHarves,
                Product = product,
                HoneyType = honeyType,
                Note = note,
                Amount = amount,
            };

            await this.harvestRepository.AddAsync(harvest);
            await this.harvestRepository.SaveChangesAsync();

            return harvest.Id;
        }

        public async Task<int> EditHarvestAsync(
            int harvestId,
            string harvestName,
            DateTime dateOfHarves,
            string product,
            HoneyType honeyType,
            string note,
            int amount)
        {
            var harvest = this.harvestRepository.All().FirstOrDefault(h => h.Id == harvestId);

            harvest.HarvestName = harvestName;
            harvest.DateOfHarves = dateOfHarves;
            harvest.Product = product;
            harvest.HoneyType = honeyType;
            harvest.Note = note;
            harvest.Amount = amount;

            await this.harvestRepository.SaveChangesAsync();
            return harvestId;
        }

        public async Task DeleteHarvestAsync(int harvestId)
        {
            var harvest = this.harvestRepository.All().FirstOrDefault(h => h.Id == harvestId);
            this.harvestRepository.Delete(harvest);
            await this.harvestRepository.SaveChangesAsync();
        }

        public T GetHarvestById<T>(int harvestId) =>
            this.harvestRepository.All()
            .Where(h => h.Id == harvestId)
            .To<T>()
            .FirstOrDefault();

        public IEnumerable<T> GetAllUserHarvests<T>(string userId) =>
            this.harvestRepository.All()
            .To<T>()
            .ToList();
    }
}
