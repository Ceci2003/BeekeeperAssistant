namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Web.ViewModels.Beehives;

    public interface IBeehiveService
    {
        IEnumerable<T> GetAllBeehives<T>(string userId);

        IEnumerable<T> GetAllBeehivesByApiary<T>(int apiaryId, string userId);

        T GetBeehiveById<T>(int id, string userId);

        Task Add(CreateBeehiveInputModel inputModel, string userId);

        Task Edit(int id, EditBeehiveInputModel inputModel, string userId);

        Task Delete(int id, string userId);

        bool Exists(int beehiveNumber, int apisryId, string userId);
    }
}
