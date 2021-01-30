namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using Microsoft.AspNetCore.Identity;

    public class ApiaryService : IApiaryService
    {
        private readonly IDeletableEntityRepository<Apiary> apiaryRepository;

        public ApiaryService(
            IDeletableEntityRepository<Apiary> apiaryRepository)
        {
            this.apiaryRepository = apiaryRepository;
        }

        public async Task Add(CreateApiaryInputModel inputModel, string userId)
        {
            var apiary = new Apiary()
            {
                Adress = inputModel.Adress,
                Name = inputModel.Name,
                Number = inputModel.Number,
                ApiaryType = inputModel.ApiaryType,
                CreatorId = userId,
            };

            await this.apiaryRepository.AddAsync(apiary);
            await this.apiaryRepository.SaveChangesAsync();
        }

        public async Task EditById(int id, EditApiaryInputModel inputModel, string userId)
        {
            var apiary = this.apiaryRepository.All().Where(a => a.Id == id && a.CreatorId == userId).FirstOrDefault();

            if (apiary != null)
            {
                apiary.Number = inputModel.Number;
                apiary.Name = inputModel.Name;
                apiary.ApiaryType = inputModel.ApiaryType;
                apiary.Adress = inputModel.Adress;

                this.apiaryRepository.Update(apiary);
                await this.apiaryRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteById(int id, string userId)
        {
            var apiary = this.apiaryRepository.All().Where(a => a.Id == id && a.CreatorId == userId).FirstOrDefault();
            if (apiary == null)
            {
                return;
            }

            this.apiaryRepository.Delete(apiary);
            await this.apiaryRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllByUser<T>(string userId)
        {
            var apiaries = this.apiaryRepository.All().Where(a => a.CreatorId == userId).To<T>().ToList();
            return apiaries;
        }

        public T GetById<T>(int id, string userId)
        {
            var apiary = this.apiaryRepository.All().Where(a => a.Id == id && a.CreatorId == userId).To<T>().FirstOrDefault();
            return apiary;
        }

        public T GetByNUmber<T>(string number, string userId)
        {
            var apiary = this.apiaryRepository.All().Where(a => a.Number == number && a.CreatorId == userId).To<T>().FirstOrDefault();
            return apiary;
        }

        public bool Exists(string apiaryNumber, string userId)
        {
            var apiary = this.apiaryRepository.All().Where(a => a.Number == apiaryNumber && a.CreatorId == userId).FirstOrDefault();

            if (apiary == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool Exists(int id, string userId)
        {
            var apiary = this.apiaryRepository.All().Where(a => a.Id == id && a.CreatorId == userId).FirstOrDefault();

            if (apiary == null)
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
