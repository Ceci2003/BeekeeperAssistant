namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Web.ViewModels.Beehives;

    public interface IBeehiveService
    {
        IEnumerable<T> GetAllUserBeehivesByApiaryId<T>(int apiaryId);

        Beehive GetBeehiveById(int id);

        T GetUserBeehiveById<T>(int id, ApplicationUser user);

        Task DeleteById(int id, ApplicationUser user);

        Task EditUserApiaryById(int id, int apiaryId, ApplicationUser user, EditBeehiveViewModel editBeehiveInputModel);

        public Task AddUserBeehive(ApplicationUser user, CreateBeehiveInputModel inputModel, int apiId);

        bool NumberExists(int number, ApplicationUser user);

        IEnumerable<T> GetAllUserBeehives<T>(ApplicationUser user);
    }
}
