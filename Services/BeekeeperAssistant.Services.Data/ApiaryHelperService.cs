namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class ApiaryHelperService : IApiaryHelperService
    {
        private readonly IRepository<ApiaryHelper> apiaryHelperRepository;
        private readonly IRepository<BeehiveHelper> beehiveHelperRepository;
        private readonly IRepository<QueenHelper> queenHelperRepository;
        private readonly IDeletableEntityRepository<Beehive> beeheiveRepository;
        private readonly IBeehiveHelperService beehiveHelperService;
        private readonly IQueenHelperService queenHelperService;
        private readonly IRepository<Apiary> apiaryRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ApiaryHelperService(
            IRepository<ApiaryHelper> apiaryHelperRepository,
            IRepository<BeehiveHelper> beehiveHelperRepository,
            IRepository<QueenHelper> queenHelperRepository,
            IDeletableEntityRepository<Beehive> beeheiveRepository,
            IBeehiveHelperService beehiveHelperService,
            IQueenHelperService queenHelperService,
            IRepository<Apiary> apiaryRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.apiaryHelperRepository = apiaryHelperRepository;
            this.beehiveHelperRepository = beehiveHelperRepository;
            this.queenHelperRepository = queenHelperRepository;
            this.beeheiveRepository = beeheiveRepository;
            this.beehiveHelperService = beehiveHelperService;
            this.queenHelperService = queenHelperService;
            this.apiaryRepository = apiaryRepository;
            this.userManager = userManager;
        }

        public async Task AddAsync(string userId, int apiaryId)
        {
            var newApiaryHelper = new ApiaryHelper
            {
                ApiaryId = apiaryId,
                UserId = userId,
                Access = Access.Read,
            };
            await this.apiaryHelperRepository.AddAsync(newApiaryHelper);
            await this.apiaryHelperRepository.SaveChangesAsync();

            var allBeehives = this.beeheiveRepository
                .All()
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

            var allQueensIds = this.beeheiveRepository
                .All()
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

        public async Task DeleteAsync(string userId, int apiaryId)
        {
            var apiary = this.apiaryHelperRepository
                .All()
                .FirstOrDefault(x => x.UserId == userId && x.ApiaryId == apiaryId);

            this.apiaryHelperRepository.Delete(apiary);
            await this.apiaryHelperRepository.SaveChangesAsync();

            var allBeehiveHelpers = this.beehiveHelperRepository
                .All()
                .Where(b => b.Beehive.ApiaryId == apiaryId && b.UserId == userId);

            foreach (var beehiveHelper in allBeehiveHelpers)
            {
                this.beehiveHelperRepository.Delete(beehiveHelper);
            }

            await this.beehiveHelperRepository.SaveChangesAsync();

            var allQueenHelpers = this.queenHelperRepository
                .All()
                .Where(q => q.Queen.Beehive.ApiaryId == apiaryId && q.UserId == userId);

            foreach (var queenHelper in allQueenHelpers)
            {
                this.queenHelperRepository.Delete(queenHelper);
            }

            await this.queenHelperRepository.SaveChangesAsync();
        }

        public async Task EditAsync(string userId, int apiaryId, Access access)
        {
            var apiaryHelper = this.apiaryHelperRepository
                .All()
                .FirstOrDefault(x => x.UserId == userId && x.ApiaryId == apiaryId);

            apiaryHelper.Access = access;

            var apiaryBeehives = this.beeheiveRepository
                .All()
                .Where(b => b.ApiaryId == apiaryId)
                .ToList();

            foreach (var beehive in apiaryBeehives)
            {
                await this.beehiveHelperService.EditAsync(userId, beehive.Id, access);

                if (beehive.QueenId.HasValue)
                {
                    await this.queenHelperService.EditAsync(userId, beehive.QueenId.Value, access);
                }
            }

            this.apiaryHelperRepository.Update(apiaryHelper);
            await this.apiaryHelperRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllApiaryHelpersByApiaryId<T>(int apiaryId, int? take = null, int skip = 0)
        {
            var qurey = this.apiaryHelperRepository
                .All()
                .Where(ah => ah.ApiaryId == apiaryId)
                .Skip(skip);

            if (take.HasValue)
            {
                qurey = qurey.Take(take.Value);
            }

            return qurey.To<T>().ToList();
        }

        public T GetApiaryHelper<T>(string userId, int apiaryId)
            => this.apiaryHelperRepository
                .All()
                .Where(x => x.UserId == userId && x.ApiaryId == apiaryId)
                .To<T>()
                .FirstOrDefault();

        public async Task<Access> GetUserApiaryAccessAsync(string userId, int apiaryId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var apiary = this.apiaryRepository.All().FirstOrDefault(a => a.Id == apiaryId);

            if (await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName) ||
                apiary.CreatorId == userId)
            {
                return Access.ReadWrite;
            }

            return this.apiaryHelperRepository.All().FirstOrDefault(ah => ah.UserId == userId && ah.ApiaryId == apiaryId).Access;
        }

        public IEnumerable<T> GetUserHelperApiaries<T>(string userId, int? take = null, int skip = 0)
        {
            var qurey = this.apiaryHelperRepository
                .All()
                .Include(x => x.Apiary)
                .ThenInclude(x => x.Creator)
                .Where(ah => ah.UserId == userId && !ah.Apiary.Creator.IsDeleted)
                .Skip(skip);

            if (take.HasValue)
            {
                qurey = qurey.Take(take.Value);
            }

            return qurey.To<T>().ToList();
        }

        public int GetUserHelperApiariesCount(string userId)
            => this.apiaryHelperRepository
                .All()
                .Where(ah => ah.UserId == userId)
                .Count();

        public int GetAllApiaryHelpersCount()
            => this.apiaryHelperRepository
            .All()
            .Where(x => !x.Apiary.IsDeleted)
            .Count();

        public bool IsApiaryHelper(string userId, int apiaryId)
        {
            var apiaryHelper = this.apiaryHelperRepository
                .All()
                .FirstOrDefault(x => x.UserId == userId && x.ApiaryId == apiaryId);

            if (apiaryHelper == null)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<T> GetAllApiaryHelpers<T>(int? take = null, int skip = 0)
        {
            var query = this.apiaryHelperRepository
                .All()
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public int GetAllApiaryHelpersWithDeletedCount()
            => this.apiaryHelperRepository
                .All()
                .Count();
    }
}
