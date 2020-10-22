namespace BeekeeperAssistant.Services.Data
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IBeehiveService
    {
        IEnumerable<T> GetAllUserBeehivesByApiaryId<T>(int apiaryId);

        bool NumberExists(int number, ApplicationUser user);

        public Task AddBeehive(ApplicationUser user, CreateBeehiveInputModel inputModel);
    }
}
