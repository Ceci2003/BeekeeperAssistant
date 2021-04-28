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
        private readonly IDeletableEntityRepository<Queen> queenRepository;
        private readonly IDeletableEntityRepository<Beehive> beehiveRepository;

        public QueenService(
            IDeletableEntityRepository<Queen> queenRepository,
            IDeletableEntityRepository<Beehive> beehiveRepository)
        {
            this.queenRepository = queenRepository;
            this.beehiveRepository = beehiveRepository;
        }

        public async Task<int> CreateUserQueenAsync(
            string userId,
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
                UserId = userId,
                QueenType = queenType,
                Temperament = temperament,
            };

            await this.queenRepository.AddAsync(queen);
            await this.queenRepository.SaveChangesAsync();

            var beehive = this.beehiveRepository.All().Where(b => b.Id == beehiveId).FirstOrDefault();
            beehive.QueenId = queen.Id;
            await this.queenRepository.SaveChangesAsync();

            return queen.Id;
        }

        public async Task DeleteQueenAsync(int queenId)
        {
            var queen = this.queenRepository.All().FirstOrDefault(q => q.Id == queenId);
            this.queenRepository.HardDelete(queen);
            await this.queenRepository.SaveChangesAsync();
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
            var queen = this.queenRepository.All().FirstOrDefault(q => q.Id == queenId);

            queen.FertilizationDate = fertilizationDate;
            queen.GivingDate = givingDate;
            queen.QueenType = queenType;
            queen.Origin = origin;
            queen.HygenicHabits = hygenicHabits;
            queen.Temperament = temperament;
            queen.Color = queenColor;
            queen.Breed = queenBreed;

            await this.queenRepository.SaveChangesAsync();
            return queen.Id;
        }

        public IEnumerable<T> GetAllUserQueens<T>(string userId, int? take = null, int skip = 0)
        {
            var query = this.queenRepository.All()
                .Where(q => q.UserId == userId).Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public int GetAllUserQueensCount(string userId)
        {
            var count = this.queenRepository.All().Where(q => q.UserId == userId).Count();
            return count;
        }

        public T GetQueenById<T>(int queenId) =>
            this.queenRepository.All()
                .Where(q => q.Id == queenId)
                .To<T>()
                .FirstOrDefault();
    }
}
