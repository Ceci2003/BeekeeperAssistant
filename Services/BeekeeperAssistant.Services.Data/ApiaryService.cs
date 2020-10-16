namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using Microsoft.AspNetCore.Identity;

    public class ApiaryService : IApiaryService
    {
        private readonly IRepository<UsersApiaries> usersApiariesRepository;
        private readonly IDeletableEntityRepository<Apiary> apiaryRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ApiaryService(
            IRepository<UsersApiaries> usersApiariesRepository,
            IDeletableEntityRepository<Apiary> apiaryRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.usersApiariesRepository = usersApiariesRepository;
            this.apiaryRepository = apiaryRepository;
            this.userManager = userManager;
        }

        public Task AddApiary()
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public void EditApiaryById()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAllApiaries<T>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAllUserApiaries<T>(string userId)
        {
            var allUserApiaries = this.usersApiariesRepository.All().Where(ua => ua.UserId == userId).To<T>().ToList();
            return allUserApiaries;
        }

        public T GetApiaryById<T>(int id)
        {
            throw new NotImplementedException();
        }

        public T GetApiaryByNumber<T>(string number, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public int GetApiaryIdByNumber(string apiNumber, ApplicationUser user)
        {
            var apiId = this.usersApiariesRepository.All().Where(ua => ua.UserId == user.Id && ua.Apiary.Number == apiNumber).FirstOrDefault().ApiaryId;
            return apiId;
        }
    }
}
