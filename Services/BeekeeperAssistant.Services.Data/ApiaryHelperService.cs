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

        public async Task Delete(string userId, int apiaryId)
        {
            var apiary = this.apiaryHelperRepository.All()
                .FirstOrDefault(x => x.UserId == userId && x.ApiaryId == apiaryId);

            this.apiaryHelperRepository.Delete(apiary);
            await this.apiaryHelperRepository.SaveChangesAsync();

            var allBeehiveHelpers = this.beehiveHelperRepository.All()
                .Where(b => b.Beehive.ApiaryId == apiaryId && b.UserId == userId);

            foreach (var beehiveHelper in allBeehiveHelpers)
            {
                this.beehiveHelperRepository.Delete(beehiveHelper);
            }

            await this.beehiveHelperRepository.SaveChangesAsync();

            var allQueenHelpers = this.queenHelperRepository.All()
                .Where(q => q.Queen.Beehive.ApiaryId == apiaryId && q.UserId == userId);

            foreach (var queenHelper in allQueenHelpers)
            {
                this.queenHelperRepository.Delete(queenHelper);
            }

            await this.queenHelperRepository.SaveChangesAsync();
        }

        public async Task Edit(string userId, int apiaryId, Access access)
        {
            var apiaryHelper = this.apiaryHelperRepository.All()
                .FirstOrDefault(x => x.UserId == userId && x.ApiaryId == apiaryId);

            apiaryHelper.Access = access;

            this.apiaryHelperRepository.Update(apiaryHelper);
            await this.apiaryHelperRepository.SaveChangesAsync();
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

        public T GetApiaryHelper<T>(string userId, int apiaryId)
        {
            var apiaryHelper = this.apiaryHelperRepository.All()
                .Where(x => x.UserId == userId && x.ApiaryId == apiaryId)
                .To<T>()
                .FirstOrDefault();

            return apiaryHelper;
        }

        public Access GetUserApiaryAccess(string userId, int apiaryId)
        {

            // TODO: Make everywhere like this!!!
            var apiaryInfo = this.apiaryHelperRepository.All()
                .Select(a => new
                {
                    CreatorId = a.Apiary.CreatorId,
                    HelperId = a.UserId,
                    ApiaryId = a.ApiaryId,
                    ApiaryAccess = a.Access,
                })
                .FirstOrDefault(a => a.HelperId == userId && a.ApiaryId == apiaryId);

            if (apiaryInfo.CreatorId == userId)
            {
                return Access.ReadWrite;
            }

            return apiaryInfo.ApiaryAccess;
        }

        public IEnumerable<T> GetUserHelperApiaries<T>(string userId, int? take = null, int skip = 0)
        {
            var qurey = this.apiaryHelperRepository
                .AllAsNoTracking()
                .Where(ah => ah.UserId == userId)
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
