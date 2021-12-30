using BeekeeperAssistant.Data.Common.Repositories;
using BeekeeperAssistant.Data.Models;
using System.Linq;
using BeekeeperAssistant.Services.Mapping;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Services.Data
{
    public class ApiaryDiaryService : IApiaryDiaryService
    {
        private readonly IRepository<ApiaryDiary> apiaryDiaryRepository;

        public ApiaryDiaryService(IRepository<ApiaryDiary> apiaryDiaryRepository)
        {
            this.apiaryDiaryRepository = apiaryDiaryRepository;
        }

        public async Task<int> CreateAsync(int apiaryId, string content, string modifiedById)
        {
            var apiaryDiary = new ApiaryDiary
            {
                ApiaryId = apiaryId,
                Content = content,
                ModifiendById = modifiedById,
            };

            await this.apiaryDiaryRepository.AddAsync(apiaryDiary);
            await this.apiaryDiaryRepository.SaveChangesAsync();

            return apiaryDiary.ApiaryId;
        }

        public T GetApiaryDiaryByApiaryId<T>(int apiaryId)
        {
            var apiaryDiary = this.apiaryDiaryRepository.All()
                .Where(ad => ad.ApiaryId == apiaryId)
                .To<T>()
                .FirstOrDefault();

            return apiaryDiary;
        }

        public async Task<int> SaveAsync(int apiaryId, string content, string modifiedById)
        {
            var apiaryDiary = this.apiaryDiaryRepository.All().FirstOrDefault(ad => ad.ApiaryId == apiaryId);

            apiaryDiary.Content = content;
            apiaryDiary.ModifiendById = modifiedById;

            this.apiaryDiaryRepository.Update(apiaryDiary);
            await this.apiaryDiaryRepository.SaveChangesAsync();

            return apiaryDiary.ApiaryId;
        }
    }
}
