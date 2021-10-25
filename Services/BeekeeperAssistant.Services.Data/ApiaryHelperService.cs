namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class ApiaryHelperService : IApiaryHelperService
    {
        private readonly IRepository<ApiaryHelper> apiaryHelperRepository;
        private readonly IRepository<BeehiveHelper> beehiveHelperRepository;
        private readonly IRepository<QueenHelper> queenHelperRepository;
        private readonly IDeletableEntityRepository<Beehive> beeheiveRepository;

        public ApiaryHelperService(
            IRepository<ApiaryHelper> apiaryHelperRepository,
            IRepository<BeehiveHelper> beehiveHelperRepository,
            IRepository<QueenHelper> queenHelperRepository,
            IDeletableEntityRepository<Beehive> beeheiveRepository)
        {
            this.apiaryHelperRepository = apiaryHelperRepository;
            this.beehiveHelperRepository = beehiveHelperRepository;
            this.queenHelperRepository = queenHelperRepository;
            this.beeheiveRepository = beeheiveRepository;
        }

        public async Task Add(string userId, int apiaryId)
        {
            var newApiaryHelper = new ApiaryHelper
            {
                ApiaryId = apiaryId,
                UserId = userId,
                Access = Access.Read,
            };
            await this.apiaryHelperRepository.AddAsync(newApiaryHelper);
            await this.apiaryHelperRepository.SaveChangesAsync();

            var allBeehives = this.beeheiveRepository.All()
                .Where(b => b.ApiaryId == apiaryId)
                .ToList();

            foreach (var beehive in allBeehives)
            {
                var helper = new BeehiveHelper
                {
                    Access = Access.Read,
                    BeehiveId = beehive.Id,
                    UserId = userId,
                };
                await this.beehiveHelperRepository.AddAsync(helper);
            }

            await this.beehiveHelperRepository.SaveChangesAsync();

            var allQueensIds = this.beeheiveRepository.All()
                .Where(b => b.ApiaryId == apiaryId)
                .Select(b => b.QueenId)
                .ToList();

            foreach (var queenId in allQueensIds)
            {
                if (queenId.HasValue)
                {
                    var helper = new QueenHelper
                    {
                        QueenId = queenId.Value,
                        UserId = userId,
                        Access = Access.Read,
                    };
                    await this.queenHelperRepository.AddAsync(helper);
                }
            }

            await this.queenHelperRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllApiaryHelpersByApiaryId<T>(int apiaryId, int? take = null, int skip = 0)
        {
            var qurey = this.apiaryHelperRepository
                .AllAsNoTracking()
                .Where(ah => ah.ApiaryId == apiaryId)
                .Skip(skip);

            if (take.HasValue)
            {
                qurey = qurey.Take(take.Value);
            }

            return qurey.To<T>().ToList();
        }

        public IEnumerable<T> GetUserHelperApiaries<T>(string userId, int? take = null, int skip = 0)
        {
            var qurey = this.apiaryHelperRepository
                .AllAsNoTracking()
                .Where(ah => ah.UserId == userId)
                .Select(ah => ah.Apiary)
                .Skip(skip);

            if (take.HasValue)
            {
                qurey = qurey.Take(take.Value);
            }

            return qurey.To<T>().ToList();
        }

        public int GetUserHelperApiariesCount(string userId)
        {
            var qurey = this.apiaryHelperRepository
                .AllAsNoTracking()
                .Where(ah => ah.UserId == userId)
                .Count();

            return qurey;
        }

        public bool IsApiaryHelper(string userId, int apiaryId)
        {
            var apiaryHelper = this.apiaryHelperRepository.All().FirstOrDefault(x => x.UserId == userId && x.ApiaryId == apiaryId);

            if (apiaryHelper == null)
            {
                return false;
            }

            return true;
        }
    }
}
