namespace BeekeeperAssistant.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class BeehiveDiaryService : IBeehiveDiaryService
    {
        private readonly IDeletableEntityRepository<BeehiveDiary> beehiveDiaryRepository;

        public BeehiveDiaryService(IDeletableEntityRepository<BeehiveDiary> beehiveDiaryRepository)
        {
            this.beehiveDiaryRepository = beehiveDiaryRepository;
        }

        public async Task<int> CreateAsync(int beehiveId, string content, string modifiedById)
        {
            var beehiveDiary = new BeehiveDiary
            {
                BeehiveId = beehiveId,
                Content = content,
                ModifiendById = modifiedById,
            };

            await this.beehiveDiaryRepository.AddAsync(beehiveDiary);
            await this.beehiveDiaryRepository.SaveChangesAsync();

            return beehiveDiary.BeehiveId;
        }

        public T GetBeehiveDiaryByBeehiveId<T>(int beehiveId)
        {
            var beehiveDiary = this.beehiveDiaryRepository.All()
                .Where(bd => bd.BeehiveId == beehiveId)
                .To<T>()
                .FirstOrDefault();

            return beehiveDiary;
        }

        public async Task<int> SaveAsync(int beehiveId, string content, string modifiedById)
        {
            var beehiveDiary = this.beehiveDiaryRepository.All().FirstOrDefault(bd => bd.BeehiveId == beehiveId);

            beehiveDiary.Content = content;
            beehiveDiary.ModifiendById = modifiedById;

            this.beehiveDiaryRepository.Update(beehiveDiary);
            await this.beehiveDiaryRepository.SaveChangesAsync();

            return beehiveDiary.BeehiveId;
        }
    }
}
