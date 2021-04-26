using System.Threading.Tasks;

namespace BeekeeperAssistant.Services
{
    public interface IForecastService
    {
        Task<ForecastResult> GetCurrentWeather(string cityName, string apiId);
    }
}
