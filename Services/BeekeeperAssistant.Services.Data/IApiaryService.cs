namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Text;

    public interface IApiaryService
    {
        IEnumerable<T> GetAllApiaries<T>();

        T GetApiaryById<T>(int id);

        IEnumerable<T> GetAllUserApiaries<T>(string userId);
    }
}
