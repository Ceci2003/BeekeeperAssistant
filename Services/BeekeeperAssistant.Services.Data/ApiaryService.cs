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

        public ApiaryService(
            IDeletableEntityRepository<Apiary> apiaryRepository,
            IDeletableEntityRepository<Beehive> beehiveRepository)
        {
            this.apiaryRepository = apiaryRepository;
            this.beehiveRepository = beehiveRepository;
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
            var apiary = this.apiaryRepository.All()
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
            var apiary = this.apiaryRepository.All()
                .FirstOrDefault(a => a.Id == apiaryId);

            apiary.Number = number;
            apiary.Name = name;
            apiary.ApiaryType = apiaryType;
            apiary.Adress = address;

            await this.apiaryRepository.SaveChangesAsync();

            return apiary.Number;
        }

        // TODO: Add pagination
        public IEnumerable<T> GetAllUserApiaries<T>(string userId, int page = 1) =>
            this.apiaryRepository.All()
                .Where(a => a.CreatorId == userId)
                .To<T>()
                .ToList();

        public T GetApiaryById<T>(int apiaryId) =>
            this.apiaryRepository.All()
                .Where(a => a.Id == apiaryId)
                .To<T>()
                .FirstOrDefault();

        public string GetApiaryNumberByBeehiveId(int beehiveId) =>
            this.beehiveRepository.All()
                .Include(a => a.Apiary)
                .FirstOrDefault(b => b.Id == beehiveId)
                .Apiary
                .Number;

        public IEnumerable<KeyValuePair<int, string>> GetUserApiariesAsKeyValuePairs(string userId) =>
            this.apiaryRepository.All()
                .Where(a => a.CreatorId == userId)
                .OrderByDescending(a => a.CreatedOn)
                .Select(a => new KeyValuePair<int, string>(a.Id, a.Number))
                .ToList();

        public T GetUserApiaryByNumber<T>(string userId, string number) =>
            this.apiaryRepository.All()
                .Where(a => a.Number == number && a.CreatorId == userId)
                .To<T>()
                .FirstOrDefault();

        public int GetUserApiaryIdByNumber(string userId, string apiaryNumber)
        {
            var apiary = this.apiaryRepository.All()
                .FirstOrDefault(a => a.CreatorId == userId && a.Number == apiaryNumber);

            return apiary.Id;
        }
    }
}
