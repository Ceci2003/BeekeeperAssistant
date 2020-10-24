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

        public async Task AddUserBeehive(ApplicationUser user, CreateBeehiveInputModel inputModel)
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
            };

            await this.beehiveRepository.AddAsync(beehive);
            await this.beehiveRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllUserBeehivesByApiaryId<T>(int apiaryId)
        {
            var allBeehives = this.beehiveRepository.All().Where(a => a.ApiaryId == apiaryId).To<T>().ToList();
            return allBeehives;
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
