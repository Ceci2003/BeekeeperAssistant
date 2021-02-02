namespace BeekeeperAssistant.Services.Data
{
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class BeehiveService : IBeehiveService
    {
        public IEnumerable<T> GetAllBeehives<T>(string userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAllBeehivesByApiary<T>(int apiaryId, string userId)
        {
            throw new NotImplementedException();
        }

        public T GetBeehiveById<T>(int id, string userId)
        {
            throw new NotImplementedException();
        }

        public Task Add(CreateBeehiveInputModel inputModel, string userId)
        {
            throw new NotImplementedException();
        }

        public Task Edit(int id, EditBeehiveInputModel inputModel, string userId)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id, string userId)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int beehiveNumber, int apisryId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
