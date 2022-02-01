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
    using Microsoft.EntityFrameworkCore;

    public class TemporaryApiaryBeehiveService : ITemporaryApiaryBeehiveService
    {
        private readonly IDeletableEntityRepository<TemporaryApiaryBeehive> temporaryApiaryBeehiveRepository;

        public TemporaryApiaryBeehiveService(
            IDeletableEntityRepository<TemporaryApiaryBeehive> temporaryApiaryBeehiveRepository)
        {
            this.temporaryApiaryBeehiveRepository = temporaryApiaryBeehiveRepository;
        }

        public T GetApiaryById<T>(int apiaryId) =>
            this.temporaryApiaryBeehiveRepository
                .All()
                .Where(ab => ab.ApiaryId == apiaryId)
                .To<T>()
                .FirstOrDefault();

        public async Task AddBeehiveToApiary(int apiaryId, int beehiveId)
        {
            var temporary = new TemporaryApiaryBeehive
            {
                ApiaryId = apiaryId,
                BeehiveId = beehiveId,
            };

            await this.temporaryApiaryBeehiveRepository.AddAsync(temporary);
            await this.temporaryApiaryBeehiveRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetBeehivesByApiaryId<T>(int apiaryId, int? take = null, int skip = 0)
        {
            var query = this.temporaryApiaryBeehiveRepository
                .All()
                .OrderByDescending(ab => ab.Apiary.Number)
                .ThenBy(ab => ab.Beehive.Number)
                .Where(ab => ab.ApiaryId == apiaryId)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }
    }
}
