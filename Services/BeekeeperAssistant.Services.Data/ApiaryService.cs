namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
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

        public async Task AddApiary(ApplicationUser user, CreateApiaryInputModel inputModel)
        {
            var apiary = new Apiary()
            {
                Adress = inputModel.Adress,
                Name = inputModel.Name,
                Number = inputModel.Number,
                ApiaryType = inputModel.ApiaryType,
                CreatorId = user.Id,
            };

            await this.apiaryRepository.AddAsync(apiary);
            await this.apiaryRepository.SaveChangesAsync();
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
            var allUserApiaries = this.apiaryRepository.All().Where(ua => ua.CreatorId == userId).To<T>().ToList();
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

        public Apiary GetApiaryByNumber(string apiNumber, ApplicationUser user)
        {
            var apiId = this.apiaryRepository.All().
                Where(a => a.CreatorId == user.Id && a.Number == apiNumber).
                FirstOrDefault();

            return apiId;
        }

        public bool ApiaryExists(string apiNumber, ApplicationUser user)
        {
            var api = this.apiaryRepository.All().Where(a => a.Number == apiNumber && a.CreatorId == user.Id).FirstOrDefault();
            if (api == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
