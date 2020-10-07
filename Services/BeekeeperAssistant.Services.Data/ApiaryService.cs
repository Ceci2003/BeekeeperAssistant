namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class ApiaryService : IApiaryService
    {
        private readonly IDeletableEntityRepository<Apiary> apiaryRepository;
        private readonly IRepository<UserApiaries> userApiariesRepositories;

        public ApiaryService(
            IDeletableEntityRepository<Apiary> aiparyRepository,
            IRepository<UserApiaries> userApiariesRepositories)
        {
            this.apiaryRepository = aiparyRepository;
            this.userApiariesRepositories = userApiariesRepositories;
        }

        public IEnumerable<T> GetAllApiaries<T>()
        {
            var allApiaries = this.apiaryRepository.All().To<T>().ToList();
            return allApiaries;
        }

        public IEnumerable<T> GetAllUserApiaries<T>(string userId)
        {
            var userApiaries = this.userApiariesRepositories.All().Where(a => a.UserId == userId).To<T>().ToList();
            return userApiaries;
        }

        public T GetApiaryById<T>(int id)
        {
            var apiary = this.apiaryRepository.All().Where(a => a.Id == id).To<T>().FirstOrDefault();
            return apiary;
        }
    }
}
