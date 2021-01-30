namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;

    public interface IApiaryService
    {
        IEnumerable<T> GetAllByUser<T>(string userId);

        T GetByNUmber<T>(string number, string userId);

        T GetById<T>(int id, string userId);

        Task Add(CreateApiaryInputModel inputModel, string userId);

        Task EditById(int id, EditApiaryInputModel inputModel, string userId);

        Task DeleteById(int id, string userId);

        bool Exists(string apiaryNumber, string userId);

        bool Exists(int id, string userId);
    }
}

