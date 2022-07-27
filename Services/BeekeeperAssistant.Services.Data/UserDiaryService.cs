namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class UserDiaryService : IUserDiaryService
    {
        private readonly IDeletableEntityRepository<UserDiary> userDiaryRepository;

        public UserDiaryService(
            IDeletableEntityRepository<UserDiary> userDiaryRepository)
        {
            this.userDiaryRepository = userDiaryRepository;
        }

        public async Task<int> CreateAsync(string content, string userId)
        {
            var diary = new UserDiary
            {
                Content = content,
                UserId = userId,
            };

            await this.userDiaryRepository.AddAsync(diary);
            await this.userDiaryRepository.SaveChangesAsync();

            return diary.Id;
        }

        public T GetDiaryByUserId<T>(string userId)
        {
            var diary = this.userDiaryRepository
                .All()
                .Where(d => d.UserId == userId)
                .To<T>()
                .FirstOrDefault();

            return diary;
        }

        public async Task<int> SaveAsync(string userId, string content)
        {
            var diary = this.userDiaryRepository
                .All()
                .FirstOrDefault(d => d.UserId == userId);

            diary.Content = content;

            this.userDiaryRepository.Update(diary);
            await this.userDiaryRepository.SaveChangesAsync();

            return diary.Id;
        }

        public bool HasDiary(string userId)
        {
            return this.userDiaryRepository.All().Any(d => d.UserId == userId);
        }
    }
}
