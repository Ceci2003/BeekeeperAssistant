namespace BeekeeperAssistant.Services.Data
{
    using System;
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
        private readonly IBeehiveService beehiveService;
        private readonly IApiaryDiaryService apiaryDiaryService;
        private readonly IRepository<ApiaryDiary> apiaryDiaryRepository;

        public ApiaryService(
            IDeletableEntityRepository<Apiary> apiaryRepository,
            IDeletableEntityRepository<Beehive> beehiveRepository,
            IDeletableEntityRepository<Queen> queenRepository,
            IRepository<ApiaryHelper> apiaryHelpersReposiitory,
            IRepository<BeehiveHelper> beehiveHelpersReposiitory,
            IRepository<QueenHelper> queenHelpersReposiitory,
            IBeehiveService beehiveService,
            IApiaryDiaryService apiaryDiaryService,
            IRepository<ApiaryDiary> apiaryDiaryRepository)
        {
            this.apiaryRepository = apiaryRepository;
            this.beehiveRepository = beehiveRepository;
            this.queenRepository = queenRepository;
            this.apiaryHelpersReposiitory = apiaryHelpersReposiitory;
            this.beehiveHelpersReposiitory = beehiveHelpersReposiitory;
            this.queenHelpersReposiitory = queenHelpersReposiitory;
            this.beehiveService = beehiveService;
            this.apiaryDiaryService = apiaryDiaryService;
            this.apiaryDiaryRepository = apiaryDiaryRepository;
        }

        public async Task<int> CreateUserApiaryAsync(
            string userId,
            string number,
            string name,
            ApiaryType apiaryType,
            string adress,
            bool isRegistered,
            bool isClosed,
            DateTime? openingDate,
            DateTime? closingDate)
        {
            var newApiary = new Apiary
            {
                CreatorId = userId,
                Number = number,
                Name = name,
                ApiaryType = apiaryType,
                Adress = adress,
                IsRegistered = isRegistered,
                IsClosed = isClosed,
                OpeningDate = openingDate ?? DateTime.Today,
                ClosingDate = closingDate ?? DateTime.Today,
            };

            await this.apiaryRepository.AddAsync(newApiary);
            await this.apiaryRepository.SaveChangesAsync();

            await this.apiaryDiaryService.CreateAsync(newApiary.Id, string.Empty, newApiary.CreatorId);

            return newApiary.Id;
        }

        public async Task<int> EditApiaryByIdAsync(
            int apiaryId,
            string number,
            string name,
            ApiaryType apiaryType,
            string address,
            bool isRegistered,
            bool isClosed,
            DateTime openingDate,
            DateTime closingDate)
        {
            var apiary = this.apiaryRepository
                .All()
                .FirstOrDefault(a => a.Id == apiaryId);

            apiary.Number = number;
            apiary.Name = name;
            apiary.ApiaryType = apiaryType;
            apiary.Adress = address;
            apiary.IsRegistered = isRegistered;
            apiary.IsClosed = isClosed;
            apiary.OpeningDate = openingDate;
            apiary.ClosingDate = closingDate;

            await this.apiaryRepository.SaveChangesAsync();

            return apiary.Id;
        }

        public async Task DeleteApiaryByIdAsync(int apiaryId)
        {
            var apiary = this.apiaryRepository
                .All()
                .Include(a => a.Beehives)
                .FirstOrDefault(a => a.Id == apiaryId);

            this.apiaryRepository.Delete(apiary);
            await this.apiaryRepository.SaveChangesAsync();
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
                .Where(a => a.CreatorId == userId && a.ApiaryType != ApiaryType.Movable)
                .Skip(skip);

            if (take.HasValue)
            {
                qurey = qurey.Take(take.Value);
            }

            return qurey.To<T>().ToList();
        }

        public IEnumerable<T> GetAllUserMovableApiaries<T>(string userId, int? take = null, int skip = 0)
        {
            var qurey = this.apiaryRepository
                .All()
                .OrderByDescending(x => x.IsBookMarked)
                .ThenByDescending(x => x.CreatedOn)
                .Where(a => a.CreatorId == userId && a.ApiaryType == ApiaryType.Movable)
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
                .Select(a => a.IsRegistered ? new KeyValuePair<int, string>(a.Id, a.Number) : new KeyValuePair<int, string>(a.Id, a.Name))
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

        public string GetApiaryOwnerIdByApiaryId(int apiaryId)
            => this.apiaryRepository
                .All()
                .FirstOrDefault(x => x.Id == apiaryId).CreatorId;

        public int AllApiariesCount()
            => this.apiaryRepository
                .All()
                .Count();

        public IEnumerable<T> GetAllApiaries<T>()
            => this.apiaryRepository
                .All()
                .To<T>()
                .ToList();

        public IEnumerable<T> GetAllApiariesWithDeleted<T>(int? take = null, int skip = 0)
        {
            var query = this.apiaryRepository
                .AllWithDeleted()
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public async Task UndeleteAsync(int apiaryId)
        {
            var apiary = this.apiaryRepository
                .AllWithDeleted()
                .FirstOrDefault(a => a.Id == apiaryId);

            this.apiaryRepository.Undelete(apiary);
            await this.apiaryRepository.SaveChangesAsync();
        }

        public int GetAllApiariesWithDeletedCount()
        => this.apiaryRepository
                .AllWithDeleted()
                .Count();

        public bool HasDiary(int apiaryId)
        {
            return this.apiaryDiaryRepository.All().Any(ad => ad.ApiaryId == apiaryId);
        }

        public async Task UpdateMovableStatus(int apiaryId)
        {
            var apiary = this.apiaryRepository
                .AllWithDeleted()
                .FirstOrDefault(a => a.Id == apiaryId);

            if (apiary == null)
            {
                return;
            }

            apiary.IsClosed = !apiary.IsClosed;

            await this.apiaryRepository.SaveChangesAsync();
        }
    }
}
