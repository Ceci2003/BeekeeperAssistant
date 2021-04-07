namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class ApiaryService : IApiaryService
    {
        private readonly IDeletableEntityRepository<Apiary> apiaryRepository;

        public ApiaryService(IDeletableEntityRepository<Apiary> apiaryRepository)
        {
            this.apiaryRepository = apiaryRepository;
        }

        public async Task<string> CreateUserApiaryAsync(string userId, string number, string name, ApiaryType apiaryType, string adress)
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

        public async Task<string> EditApiaryByIdAsync(int apiaryId, string number, string name, ApiaryType apiaryType, string address)
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
        public IEnumerable<T> GetAllUserApiaries<T>(string userId, int page = 1)
        {
            var userApiaries = this.apiaryRepository.All()
                .Where(a => a.CreatorId == userId)
                .To<T>()
                .ToList();

            return userApiaries;
        }

        public T GetApiaryById<T>(int apiaryId)
        {
            var apiary = this.apiaryRepository.All()
                .Where(a => a.Id == apiaryId)
                .To<T>()
                .FirstOrDefault();

            return apiary;
        }

        public T GetUserApiaryByNumber<T>(string userId, string number)
        {
            var apiary = this.apiaryRepository.All()
                .Where(a => a.Number == number && a.CreatorId == userId)
                .To<T>()
                .FirstOrDefault();

            return apiary;
        }
    }
}



// GetAllUserApiaries + pagination
// GetUserApiaryByNumber
// GetApairyById
// CreateUserApiary
// DeleteUserApiaryById / DeleteUserApiaryByNumber
// EditUserApiaryByNumber / EditUserApiaryById
