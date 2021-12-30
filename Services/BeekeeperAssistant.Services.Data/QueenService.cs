namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class QueenService : IQueenService
    {
        private readonly IRepository<ApiaryHelper> apiaryHelperRepository;
        private readonly IRepository<QueenHelper> queenHelperRepository;
        private readonly IDeletableEntityRepository<Queen> queenRepository;
        private readonly IDeletableEntityRepository<Beehive> beehiveRepository;

        public QueenService(
            IRepository<ApiaryHelper> apiaryHelperRepository,
            IRepository<QueenHelper> queenHelperRepository,
            IDeletableEntityRepository<Queen> queenRepository,
            IDeletableEntityRepository<Beehive> beehiveRepository)
        {
            this.apiaryHelperRepository = apiaryHelperRepository;
            this.queenHelperRepository = queenHelperRepository;
            this.queenRepository = queenRepository;
            this.beehiveRepository = beehiveRepository;
        }

        public async Task BookmarkQueenAsync(int queenId)
        {
            var queen = this.queenRepository
                .All()
                .FirstOrDefault(q => q.Id == queenId);

            if (queen == null)
            {
                return;
            }

            queen.IsBookMarked = !queen.IsBookMarked;
            await this.queenRepository.SaveChangesAsync();
        }

        public async Task<int> CreateUserQueenAsync(
            string creatorId,
            int beehiveId,
            DateTime fertilizationDate,
            DateTime givingDate,
            QueenType queenType,
            string origin,
            string hygenicHabits,
            string temperament,
            QueenColor queenColor,
            QueenBreed queenBreed)
        {
            var queen = new Queen
            {
                BeehiveId = beehiveId,
                Breed = queenBreed,
                Color = queenColor,
                FertilizationDate = fertilizationDate,
                GivingDate = givingDate,
                HygenicHabits = hygenicHabits,
                Origin = origin,
                CreatorId = creatorId,
                QueenType = queenType,
                Temperament = temperament,
            };

            await this.queenRepository.AddAsync(queen);
            await this.queenRepository.SaveChangesAsync();

            var beehive = this.beehiveRepository
                .All()
                .Where(b => b.Id == beehiveId)
                .FirstOrDefault();

            beehive.QueenId = queen.Id;
            await this.queenRepository.SaveChangesAsync();

            var apiaryId = this.beehiveRepository
                .All()
                .Where(b => b.Id == beehiveId)
                .Select(b => b.ApiaryId)
                .FirstOrDefault();

            var allApiaryHelpersIds = this.apiaryHelperRepository
                .All()
                .Where(x => x.ApiaryId == apiaryId)
                .Select(x => x.UserId);

            foreach (var helperId in allApiaryHelpersIds)
            {
                var helper = new QueenHelper
                {
                    UserId = helperId,
                    QueenId = queen.Id,
                };

                await this.queenHelperRepository.AddAsync(helper);
            }

            await this.queenHelperRepository.SaveChangesAsync();

            return queen.BeehiveId;
        }

        public async Task<int> DeleteQueenAsync(int queenId)
        {
            await this.DeleteAllQueenHelpersByQueenIdAsync(queenId);

            var queen = this.queenRepository
                .All()
                .FirstOrDefault(q => q.Id == queenId);

            var beehive = this.beehiveRepository
                .All()
                .FirstOrDefault(b => b.QueenId == queen.Id);

            beehive.QueenId = null;

            this.beehiveRepository.Update(beehive);
            this.queenRepository.HardDelete(queen);

            await this.queenRepository.SaveChangesAsync();
            await this.beehiveRepository.SaveChangesAsync();

            return beehive.Id;
        }

        public async Task DeleteAllQueenHelpersByQueenIdAsync(int queenId)
        {
            var allQueenHelper = this.queenHelperRepository.All()
                .Where(q => q.QueenId == queenId);

            foreach (var helper in allQueenHelper)
            {
                this.queenHelperRepository.Delete(helper);
            }

            await this.queenHelperRepository.SaveChangesAsync();
        }

        public async Task<int> EditQueenAsync(
            int queenId,
            DateTime fertilizationDate,
            DateTime givingDate,
            QueenType queenType,
            string origin,
            string hygenicHabits,
            string temperament,
            QueenColor queenColor,
            QueenBreed queenBreed)
        {
            var queen = this.queenRepository
                .All()
                .FirstOrDefault(q => q.Id == queenId);

            queen.FertilizationDate = fertilizationDate;
            queen.GivingDate = givingDate;
            queen.QueenType = queenType;
            queen.Origin = origin;
            queen.HygenicHabits = hygenicHabits;
            queen.Temperament = temperament;
            queen.Color = queenColor;
            queen.Breed = queenBreed;

            await this.queenRepository.SaveChangesAsync();
            return queen.BeehiveId;
        }

        public IEnumerable<T> GetAllUserQueens<T>(string ownerId, int? take = null, int skip = 0)
        {
            var query = this.queenRepository
                .All()
                .OrderByDescending(q => q.IsBookMarked)
                .ThenByDescending(q => q.GivingDate)
                .ThenBy(q => q.Beehive.Apiary.Number)
                .ThenBy(q => q.Beehive.Number)
                .Where(q => q.OwnerId == ownerId && !q.Beehive.IsDeleted)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public int GetAllUserQueensCount(string ownerId)
            => this.queenRepository
                .All()
                .Where(q => q.OwnerId == ownerId && q.Beehive.IsDeleted == false)
                .Count();

        public T GetQueenByBeehiveId<T>(int beehiveId) =>
            this.queenRepository
                .All()
                .Where(q => q.BeehiveId == beehiveId)
                .To<T>()
                .FirstOrDefault();

        public T GetQueenById<T>(int queenId) =>
            this.queenRepository
                .AllAsNoTracking()
                .Where(q => q.Id == queenId)
                .To<T>()
                .FirstOrDefault();

    }
}
