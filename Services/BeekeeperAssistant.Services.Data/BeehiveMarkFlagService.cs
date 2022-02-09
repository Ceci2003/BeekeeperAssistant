namespace BeekeeperAssistant.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class BeehiveMarkFlagService : IBeehiveMarkFlagService
    {
        private readonly IRepository<BeehiveMarkFlag> beehiveMarkFlagRepositoey;

        public BeehiveMarkFlagService(
            IRepository<BeehiveMarkFlag> beehiveMarkFlagRepositoey)
        {
            this.beehiveMarkFlagRepositoey = beehiveMarkFlagRepositoey;
        }

        public async Task<int> CreateBeehiveFlag(int beehiveId, string content, MarkFlagType flagType)
        {
            var beehiveMarkFlag = new BeehiveMarkFlag()
            {
                BeehiveId = beehiveId,
                Content = content,
                FlagType = flagType,
            };

            await this.beehiveMarkFlagRepositoey.AddAsync(beehiveMarkFlag);
            await this.beehiveMarkFlagRepositoey.SaveChangesAsync();

            return beehiveMarkFlag.Id;
        }

        public async Task<int> EditBeehiveFlag(int id, string content, MarkFlagType flagType)
        {
            var beehiveMarkFlag = this.beehiveMarkFlagRepositoey
                .All()
                .FirstOrDefault(bf => bf.Id == id);

            beehiveMarkFlag.Content = content;
            beehiveMarkFlag.FlagType = flagType;

            await this.beehiveMarkFlagRepositoey.SaveChangesAsync();

            return beehiveMarkFlag.Id;
        }

        public async Task DeleteBeehiveFlag(int id)
        {
            var beehiveMarkFlag = this.beehiveMarkFlagRepositoey
                .All()
                .FirstOrDefault(bf => bf.Id == id);

            this.beehiveMarkFlagRepositoey.Delete(beehiveMarkFlag);
            await this.beehiveMarkFlagRepositoey.SaveChangesAsync();
        }

        public T GetBeehiveFlagByBeehiveId<T>(int beehiveId) =>
            this.beehiveMarkFlagRepositoey
                .All()
                .Where(bf => bf.BeehiveId == beehiveId)
                .To<T>()
                .FirstOrDefault();

        public T GetBeehiveFlagByFlagId<T>(int id) =>
            this.beehiveMarkFlagRepositoey
                .All()
                .Where(bf => bf.Id == id)
                .To<T>()
                .FirstOrDefault();

        public bool BeehiveHasFlag(int beehiveId)
        {
            var beehiveMarkFlag = this.beehiveMarkFlagRepositoey
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
