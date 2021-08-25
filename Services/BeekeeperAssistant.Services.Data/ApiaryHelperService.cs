namespace BeekeeperAssistant.Services.Data
{
    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using System;
    using System.Threading.Tasks;

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
                CanRead = true,
            };

            await this.apiaryHelperRepository.AddAsync(newApiaryHelper);
            await this.apiaryHelperRepository.SaveChangesAsync();
        }
    }
}
