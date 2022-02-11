namespace BeekeeperAssistant.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class BeehiveMarkFlagService : IBeehiveMarkFlagService
    {
        private readonly IRepository<BeehiveMarkFlag> beehiveMarkFlagRepository;

        public BeehiveMarkFlagService(
            IRepository<BeehiveMarkFlag> beehiveMarkFlagRepositoey)
        {
            this.beehiveMarkFlagRepository = beehiveMarkFlagRepositoey;
        }

        public async Task<int> CreateBeehiveFlag(int beehiveId, string content, MarkFlagType flagType)
        {
            var beehiveMarkFlag = new BeehiveMarkFlag()
            {
                BeehiveId = beehiveId,
                Content = content,
                FlagType = flagType,
            };

            await this.beehiveMarkFlagRepository.AddAsync(beehiveMarkFlag);
            await this.beehiveMarkFlagRepository.SaveChangesAsync();

            return beehiveMarkFlag.Id;
        }

        public async Task<int> EditBeehiveFlag(int id, string content, MarkFlagType flagType)
        {
            var beehiveMarkFlag = this.beehiveMarkFlagRepository
                .All()
                .FirstOrDefault(bf => bf.Id == id);

            beehiveMarkFlag.Content = content;
            beehiveMarkFlag.FlagType = flagType;

            await this.beehiveMarkFlagRepository.SaveChangesAsync();

            return beehiveMarkFlag.Id;
        }

        public async Task DeleteBeehiveFlag(int id)
        {
            var beehiveMarkFlag = this.beehiveMarkFlagRepository
                .All()
                .FirstOrDefault(bf => bf.Id == id);

            this.beehiveMarkFlagRepository.Delete(beehiveMarkFlag);
            await this.beehiveMarkFlagRepository.SaveChangesAsync();
        }

        public T GetBeehiveFlagByBeehiveId<T>(int beehiveId) =>
            this.beehiveMarkFlagRepository
                .All()
                .Where(bf => bf.BeehiveId == beehiveId)
                .To<T>()
                .FirstOrDefault();

        public T GetBeehiveFlagByFlagId<T>(int id) =>
            this.beehiveMarkFlagRepository
                .All()
                .Where(bf => bf.Id == id)
                .To<T>()
                .FirstOrDefault();

        public MarkFlagType? GetBeehiveFlagTypeByBeehiveId(int beehiveId)
        {
            var flagType = this.beehiveMarkFlagRepository
                .All()
                .Where(bf => bf.BeehiveId == beehiveId)
                .FirstOrDefault();

            if (flagType == null)
            {
                return null;
            }

            return flagType.FlagType;
        }

        public bool BeehiveHasFlag(int beehiveId)
        {
            var beehiveMarkFlag = this.beehiveMarkFlagRepository
                .All()
                .FirstOrDefault(bf => bf.BeehiveId == beehiveId);

            if (beehiveMarkFlag == null)
            {
                return false;
            }

            return true;
        }
    }
}
