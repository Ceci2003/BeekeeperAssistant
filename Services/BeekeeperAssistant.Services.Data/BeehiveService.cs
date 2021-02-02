namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Beehives;

    public class BeehiveService : IBeehiveService
    {
        private readonly IDeletableEntityRepository<Beehive> beehiveRepository;

        public BeehiveService(
            IDeletableEntityRepository<Beehive> beehiveRepository)
        {
            this.beehiveRepository = beehiveRepository;
        }

        public IEnumerable<T> GetAllBeehives<T>(string userId)
        {
            var beehives = this.beehiveRepository.All().Where(b => b.CreatorId == userId).To<T>().ToList();
            return beehives;
        }

        public IEnumerable<T> GetAllBeehivesByApiary<T>(int apiaryId, string userId)
        {
            var beehives = this.beehiveRepository.All().Where(b => b.ApiaryId == apiaryId && b.CreatorId == userId).To<T>().ToList();
            return beehives;
        }

        public T GetBeehiveById<T>(int id, string userId)
        {
            var beehive = this.beehiveRepository.All().Where(b => b.Id == id && b.CreatorId == userId).To<T>().FirstOrDefault();
            return beehive;
        }

        public async Task Add(CreateBeehiveInputModel inputModel, string userId)
        {
            var beehive = new Beehive()
            {
                ApiaryId = inputModel.ApiaryId,
                BeehivePower = inputModel.BeehivePower,
                BeehiveSystem = inputModel.BeehiveSystem,
                BeehiveType = inputModel.BeehiveType,
                Date = inputModel.Date,
                Number = inputModel.Number,
                HasDevice = inputModel.HasDevice,
                HasPolenCatcher = inputModel.HasPolenCatcher,
                HasPropolisCatcher = inputModel.HasPropolisCatcher,
                CreatorId = userId,
            };

            await this.beehiveRepository.AddAsync(beehive);
            await this.beehiveRepository.SaveChangesAsync();
        }

        public async Task Edit(int id, EditBeehiveInputModel inputModel, string userId)
        {
            var beehive = this.beehiveRepository.All().Where(b => b.Id == id && b.CreatorId == userId).FirstOrDefault();

            if (beehive != null)
            {
                beehive.Number = inputModel.Number;
                beehive.BeehiveSystem = inputModel.BeehiveSystem;
                beehive.BeehiveType = inputModel.BeehiveType;
                beehive.Date = inputModel.Date;
                beehive.BeehivePower = inputModel.BeehivePower;
                beehive.ApiaryId = inputModel.ApiaryId;
                beehive.HasDevice = inputModel.HasDevice;
                beehive.HasPolenCatcher = inputModel.HasPolenCatcher;
                beehive.HasPropolisCatcher = inputModel.HasPropolisCatcher;
                beehive.CreatorId = userId;

                this.beehiveRepository.Update(beehive);
                await this.beehiveRepository.SaveChangesAsync();
            }
        }

        public async Task Delete(int id, string userId)
        {
            var beehive = this.beehiveRepository.All().Where(b => b.Id == id && b.CreatorId == userId).FirstOrDefault();
            if (beehive == null)
            {
                return;
            }

            this.beehiveRepository.Delete(beehive);
            await this.beehiveRepository.SaveChangesAsync();
        }

        public bool Exists(int beehiveNumber, int apiaryId, string userId)
        {
            var beehive = this.beehiveRepository.All().Where(b => b.Number == beehiveNumber && b.ApiaryId == apiaryId && b.CreatorId == userId).FirstOrDefault();

            if (beehive == null)
            {
                return false;
            }

            return true;
        }
    }
}
