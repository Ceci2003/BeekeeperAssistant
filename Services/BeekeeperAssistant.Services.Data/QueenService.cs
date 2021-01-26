namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Queens;

    public class QueenService : IQueenService
    {
        private readonly IDeletableEntityRepository<Queen> queenReepository;

        public QueenService(IDeletableEntityRepository<Queen> queenReepository)
        {
            this.queenReepository = queenReepository;
        }

        public async Task<int> CreateQueen(CreateQueenInputModel inputModel, int beehiveId, string currentUserId)
        {
            var model = new Queen()
            {
                Breed = inputModel.QueenBreed,
                CreatorId = currentUserId,
                Color = inputModel.QueenColor,
                FertilizationDate = inputModel.FertilizationDate,
                GivingDate = inputModel.GivingDate,
                HygenicHabits = inputModel.HygenicHabits,
                Origin = inputModel.Origin,
                Temperament = inputModel.Temparament,
                QueenType = inputModel.QueenType,
                BeehiveId = beehiveId,
            };

            await this.queenReepository.AddAsync(model);
            await this.queenReepository.SaveChangesAsync();

            return model.Id;
        }

        public IEnumerable<T> GetAllQueens<T>(int beehiveId, string currentUserId)
        {
            var allBeehiveQueens = this.queenReepository.All().Where(q => q.BeehiveId == beehiveId && q.CreatorId == currentUserId).To<T>().ToList();
            return allBeehiveQueens;
        }

        public IEnumerable<T> GetAllUserQueens<T>(string currentUserId)
        {
            var allBeehiveQueens = this.queenReepository.All().Where(q => q.CreatorId == currentUserId).To<T>().ToList();
            return allBeehiveQueens;
        }

        public T GetQueenById<T>(int id)
        {
            var queen = this.queenReepository.All().Where(q => q.Id == id).To<T>().FirstOrDefault();
            return queen;
        }
    }
}
