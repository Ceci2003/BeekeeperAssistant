namespace BeekeeperAssistant.Services
{
    using System.Threading.Tasks;

    public interface IForecastService
    {
        Task<ForecastResult> GetApiaryCurrentWeatherByCityName(string cityName, string apiId);

        Task<ForecastResult> GetApiaryCurrentWeatherByCityPostcode(string postcode, string apiId);

        bool ValidateCityPostcode(string postcode);
    }
}
