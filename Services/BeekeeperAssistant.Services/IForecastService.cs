namespace BeekeeperAssistant.Services
{
    public interface IForecastService
    {
        ForecastResult GetCurrentWeather(string cityName, string apiId);
    }
}
