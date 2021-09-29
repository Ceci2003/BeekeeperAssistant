namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;

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
