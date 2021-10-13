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

        public ApiaryHelperService(IRepository<ApiaryHelper> apiaryHelperRepository)
        {
            this.apiaryHelperRepository = apiaryHelperRepository;
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

        public bool IsAnApiaryHelper(string userId, int apiaryId)
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
