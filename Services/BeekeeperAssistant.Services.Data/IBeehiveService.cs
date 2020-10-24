namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Web.ViewModels.Beehives;

    public interface IBeehiveService
    {
        IEnumerable<T> GetAllUserBeehivesByApiaryId<T>(int apiaryId);

        bool NumberExists(int number, ApplicationUser user);

        public Task AddUserBeehive(ApplicationUser user, CreateBeehiveInputModel inputModel);
    }
}
