namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Beehives;

    public class BeehiveService : IBeehiveService
    {
        private readonly IDeletableEntityRepository<Beehive> beehiveRepository;

        public BeehiveService(IDeletableEntityRepository<Beehive> beehiveRepository)
        {
            this.beehiveRepository = beehiveRepository;
        }

        public async Task AddUserBeehive(ApplicationUser user, CreateBeehiveInputModel inputModel, int apiId)
        {
            var beehive = new Beehive()
            {
                ApiaryId = apiId,
                BeehivePower = inputModel.BeehivePower,
                BeehiveSystem = inputModel.BeehiveSystem,
                BeehiveType = inputModel.BeehiveType,
                Date = inputModel.Date,
                Number = inputModel.Number,
                HasDevice = inputModel.HasDevice,
                HasPolenCatcher = inputModel.HasPolenCatcher,
                HasPropolisCatcher = inputModel.HasPropolisCatcher,
                CreatorId = user.Id,
            };

            await this.beehiveRepository.AddAsync(beehive);
            await this.beehiveRepository.SaveChangesAsync();
        }

        public Task DeleteById(int id, ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task EditUserApiaryById(int id, int apiaryId, ApplicationUser user, EditBeehiveViewModel editBeehiveInputModel)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetAllUserBeehives<T>(ApplicationUser user)
        {
            var beehives = this.beehiveRepository.All().Where(b => b.CreatorId == user.Id).To<T>().ToList();
            return beehives;
        }

        public IEnumerable<T> GetAllUserBeehivesByApiaryId<T>(int apiaryId)
        {
            var allBeehives = this.beehiveRepository.All().Where(a => a.ApiaryId == apiaryId).To<T>().ToList();
            return allBeehives;
        }

        public Beehive GetBeehiveById(int id)
        {
            var beehive = this.beehiveRepository.All().Where(b => b.Id == id).FirstOrDefault();
            return beehive;
        }

        public T GetUserBeehiveById<T>(int id, ApplicationUser user)
        {
            var beehive = this.beehiveRepository.All().Where(b => b.Id == id && b.Creator == user).To<T>().FirstOrDefault();
            return beehive;
        }

        public bool NumberExists(int number, ApplicationUser user)
        {
            var beehive = this.beehiveRepository.All().Where(b => b.Number == number && b.CreatorId == user.Id).FirstOrDefault();

            if (beehive == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
