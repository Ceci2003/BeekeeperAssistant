namespace BeekeeperAssistant.Services
{
    using System.Threading.Tasks;

    public interface IForecastService
    {
        Task<ForecastResult> GetCurrentWeather(string cityName, string apiId);
    }
}
