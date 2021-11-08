namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ApiaryService : IApiaryService
    {
        private readonly IDeletableEntityRepository<Apiary> apiaryRepository;
        private readonly IDeletableEntityRepository<Beehive> beehiveRepository;
        private readonly IDeletableEntityRepository<Queen> queenRepository;
        private readonly IRepository<ApiaryHelper> apiaryHelpersReposiitory;
        private readonly IRepository<BeehiveHelper> beehiveHelpersReposiitory;
        private readonly IRepository<QueenHelper> queenHelpersReposiitory;

        public ApiaryService(
            IDeletableEntityRepository<Apiary> apiaryRepository,
            IDeletableEntityRepository<Beehive> beehiveRepository,
            IDeletableEntityRepository<Queen> queenRepository,
            IRepository<ApiaryHelper> apiaryHelpersReposiitory,
            IRepository<BeehiveHelper> beehiveHelpersReposiitory,
            IRepository<QueenHelper> queenHelpersReposiitory)
        {
            this.apiaryRepository = apiaryRepository;
            this.beehiveRepository = beehiveRepository;
            this.queenRepository = queenRepository;
            this.apiaryHelpersReposiitory = apiaryHelpersReposiitory;
            this.beehiveHelpersReposiitory = beehiveHelpersReposiitory;
            this.queenHelpersReposiitory = queenHelpersReposiitory;
        }

        public async Task<string> CreateUserApiaryAsync(
            string userId,
            string number,
            string name,
            ApiaryType apiaryType,
            string adress)
        {
            var newApiary = new Apiary
            {
                CreatorId = userId,
                Number = number,
                Name = name,
                ApiaryType = apiaryType,
                Adress = adress,
            };

            await this.apiaryRepository.AddAsync(newApiary);
            await this.apiaryRepository.SaveChangesAsync();

            var apiaryNumber = newApiary.Number;
            return apiaryNumber;
        }

        public async Task DeleteApiaryByIdAsync(int apiaryId)
        {
            // Delete all apiaryHelpers
            var allApiaryHelpers = this.apiaryHelpersReposiitory
                .All()
                .Where(x => x.ApiaryId == apiaryId);

            foreach (var apiaryHelper in allApiaryHelpers)
            {
                this.apiaryHelpersReposiitory.Delete(apiaryHelper);
            }

            await this.apiaryHelpersReposiitory.SaveChangesAsync();

            // Delete all beehiveHelpers
            var allBeehiveHeleprs = this.apiaryRepository
                .All()
                .Where(x => x.Id == apiaryId)
                .SelectMany(x => x.Beehives)
                .SelectMany(x => x.BeehiveHelpers);

            foreach (var beehiveHelper in allBeehiveHeleprs)
            {
                this.beehiveHelpersReposiitory.Delete(beehiveHelper);
            }

            await this.beehiveHelpersReposiitory.SaveChangesAsync();

            // Delete all queenHelpers
            var allQueenHelpers = this.apiaryRepository
                .All()
                .Where(x => x.Id == apiaryId)
                .SelectMany(x => x.Beehives)
                .Select(x => x.Queen)
                .SelectMany(x => x.QueenHelpers);

            foreach (var queenHelper in allQueenHelpers)
            {
                this.queenHelpersReposiitory.Delete(queenHelper);
            }

            await this.queenHelpersReposiitory.SaveChangesAsync();

            // Delete all beehives
            var apiaryBeehives = this.beehiveRepository
                .All()
                .Include(b => b.Queen)
                .Where(b => b.ApiaryId == apiaryId)
                .ToList();

            foreach (var beehive in apiaryBeehives)
            {
                this.beehiveRepository.Delete(beehive);

                if (beehive.QueenId.HasValue)
                {
                    this.queenRepository.HardDelete(beehive.Queen);
                }
            }

            await this.beehiveRepository.SaveChangesAsync();

            // Delete apiary
            var apiary = this.apiaryRepository
                .All()
                .FirstOrDefault(a => a.Id == apiaryId);

            this.apiaryRepository.Delete(apiary);
            await this.apiaryRepository.SaveChangesAsync();
        }

        public async Task<string> EditApiaryByIdAsync(
            int apiaryId,
            string number,
            string name,
            ApiaryType apiaryType,
            string address)
        {
            var apiary = this.apiaryRepository
                .All()
                .FirstOrDefault(a => a.Id == apiaryId);

            apiary.Number = number;
            apiary.Name = name;
            apiary.ApiaryType = apiaryType;
            apiary.Adress = address;

            await this.apiaryRepository.SaveChangesAsync();

            return apiary.Number;
        }

        public int GetAllUserApiariesCount(string userId) =>
            this.apiaryRepository
                .All()
                .Where(a => a.CreatorId == userId)
                .Count();

        public IEnumerable<T> GetAllUserApiaries<T>(string userId, int? take = null, int skip = 0)
        {
            var qurey = this.apiaryRepository
                .All()
                .OrderByDescending(x => x.IsBookMarked)
                .ThenByDescending(x => x.CreatedOn)
                .Where(a => a.CreatorId == userId)
                .Skip(skip);

            if (take.HasValue)
            {
                qurey = qurey.Take(take.Value);
            }

            return qurey.To<T>().ToList();
        }

        public T GetApiaryById<T>(int apiaryId) =>
            this.apiaryRepository
                .All()
                .Where(a => a.Id == apiaryId)
                .To<T>()
                .FirstOrDefault();

        public string GetApiaryNumberByBeehiveId(int beehiveId) =>
            this.beehiveRepository
                .All()
                .Include(a => a.Apiary)
                .FirstOrDefault(b => b.Id == beehiveId)
                .Apiary
                .Number;

        public int GetApiaryIdByBeehiveId(int beehiveId) =>
            this.beehiveRepository
                .All()
                .Include(a => a.Apiary)
                .FirstOrDefault(b => b.Id == beehiveId)
                .Apiary
                .Id;

        public IEnumerable<KeyValuePair<int, string>> GetUserApiariesAsKeyValuePairs(string userId)
            => this.apiaryRepository
                .All()
                .Where(a => a.CreatorId == userId)
                .OrderByDescending(a => a.CreatedOn)
                .Select(a => new KeyValuePair<int, string>(a.Id, a.Number))
                .ToList();

        public T GetUserApiaryByNumber<T>(string userId, string number)
            => this.apiaryRepository
                .All()
                .Where(a => a.Number == number && a.CreatorId == userId)
                .To<T>()
                .FirstOrDefault();

        public int GetApiaryIdByNumber(string apiaryNumber)
            => this.apiaryRepository
                .All()
                .FirstOrDefault(a => a.Number == apiaryNumber)
                .Id;

        public string GetApiaryNumberByApiaryId(int apiaryId)
            => this.apiaryRepository
               .All()
               .FirstOrDefault(a => a.Id == apiaryId)
               .Number;

        public T GetUserApiaryByBeehiveId<T>(int beehiveId)
            => this.beehiveRepository
                .All()
                .Include(a => a.Apiary)
                .Where(b => b.Id == beehiveId)
                .Select(b => b.Apiary)
                .To<T>()
                .FirstOrDefault();

        public T GetApiaryByNumber<T>(string apiaryNumber)
            => this.apiaryRepository
                .All()
                .Where(a => a.Number == apiaryNumber)
                .To<T>()
                .FirstOrDefault();

        public bool IsApiaryCreator(string userId, int apiaryId)
        {
            var apiary = this.apiaryRepository
                .All()
                .FirstOrDefault(a => a.Id == apiaryId);

            if (apiary == null)
            {
                return false;
            }

            return apiary.CreatorId == userId;
        }

        public async Task BookmarkApiaryAsync(int apiaryId)
        {
            var apiary = this.apiaryRepository
                .All()
                .FirstOrDefault(a => a.Id == apiaryId);

            if (apiary == null)
            {
                return;
            }

            apiary.IsBookMarked = !apiary.IsBookMarked;

            await this.apiaryRepository.SaveChangesAsync();
        }

        public string GetApiaryCreatorIdByApiaryId(int apiaryId)
            => this.apiaryRepository
                .All()
                .FirstOrDefault(x => x.Id == apiaryId).CreatorId;

        public int Count()
            => this.apiaryRepository.All().Count();
    }
}
