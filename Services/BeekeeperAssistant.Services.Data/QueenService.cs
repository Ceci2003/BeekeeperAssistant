namespace BeekeeperAssistant.Services.Data
{
    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Services.Mapping;
    using System.Collections.Generic;

    public class QueenService : IQueenService
    {
        private readonly IDeletableEntityRepository<Queen> queenRepository;
        private readonly IDeletableEntityRepository<Beehive> beehiveRepository;

        public QueenService(IDeletableEntityRepository<Queen> queenRepository, IDeletableEntityRepository<Beehive> beehiveRepository)
        {
            this.queenRepository = queenRepository;
            this.beehiveRepository = beehiveRepository;
        }

        public async Task<int> CreateUserQueenAsync(string userId, DateTime fertilizationDate, DateTime givingDate, QueenType queenType, string origin, string hygenicHabits, string temperament, QueenColor queenColor, QueenBreed queenBreed, int beehiveId)
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

        public IEnumerable<T> GetAllUserQueens<T>(string userId)
        {
            var allQueens = this.queenRepository.All()
                .Where(q => q.UserId == userId)
                .To<T>()
                .ToList();

            return allQueens;
        }

        // TODO: Write all funtions with arrows =>
        public T GetQueenById<T>(int queenId)
        {
            var queen = this.queenRepository.All()
                .Where(q => q.Id == queenId)
                .To<T>()
                .FirstOrDefault();

            return queen;
        }
    }
}