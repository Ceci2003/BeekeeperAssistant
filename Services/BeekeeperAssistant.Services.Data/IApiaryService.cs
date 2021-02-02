namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Web.ViewModels.Apiaries;

    public interface IApiaryService
    {
        IEnumerable<T> GetAll<T>(string userId);

        T GetByNUmber<T>(string number, string userId);

        T GetById<T>(int id, string userId);

        Task Add(CreateApiaryInputModel inputModel, string userId);

        Task Edit(int id, EditApiaryInputModel inputModel, string userId);

        Task Delete(int id, string userId);

        bool Exists(string apiaryNumber, string userId);

        bool Exists(int id, string userId);
    }
}
