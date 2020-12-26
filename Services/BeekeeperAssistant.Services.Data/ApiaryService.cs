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

        public async Task AddUserApiary(ApplicationUser user, CreateApiaryInputModel inputModel)
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

        public async Task DeleteById(int id, ApplicationUser user)
        {
            var apiary = this.apiaryRepository.All().Where(a => a.Id == id && a.CreatorId == user.Id).FirstOrDefault();
            if (apiary == null)
            {
                return;
            }

            this.apiaryRepository.Delete(apiary);
            await this.apiaryRepository.SaveChangesAsync();
        }

        public async Task EditUserApiaryById(int id, ApplicationUser user, EditApiaryInputModel editApiaryInputModel)
        {
            var apiary = this.apiaryRepository.All().Where(a => a.Id == id && a.CreatorId == user.Id).FirstOrDefault();

            if (apiary != null)
            {
                apiary.Number = editApiaryInputModel.Number;
                apiary.Name = editApiaryInputModel.Name;
                apiary.ApiaryType = editApiaryInputModel.ApiaryType;
                apiary.Adress = editApiaryInputModel.Adress;

                this.apiaryRepository.Update(apiary);
                await this.apiaryRepository.SaveChangesAsync();
            }
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
            var apiary = this.apiaryRepository.All().Where(a => a.Id == id).To<T>().FirstOrDefault();
            return apiary;
        }

        public T GetApiaryByNumber<T>(string number, ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public T GetUserApiaryById<T>(int id, ApplicationUser user)
        {
            var apiary = this.apiaryRepository.All().Where(a => a.Id == id && a.CreatorId == user.Id).To<T>().FirstOrDefault();
            return apiary;
        }

        public Apiary GetUserApiaryByNumber(string apiNumber, ApplicationUser user)
        {
            var apiaryId = this.apiaryRepository.All().
                Where(a => a.CreatorId == user.Id && a.Number == apiNumber).
                FirstOrDefault();

            return apiaryId;
        }

        public bool UserApiaryExists(string apiNumber, ApplicationUser user)
        {
            var apiary = this.apiaryRepository.All().Where(a => a.Number == apiNumber && a.CreatorId == user.Id).FirstOrDefault();

            if (apiary == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool EditApiaryExist(string apiNumber, ApplicationUser user, int apiId)
        {
            // TODO: EditApiary Service
            // Implement with Ceci :D
            return true;
        }
    }
}
