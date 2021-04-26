namespace BeekeeperAssistant.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public class ForecastService : IForecastService
    {
        public async Task<ForecastResult> GetCurrentWeather(string cityName, string apiId)
        {
            // API path with CITY parameter and other parameters.
            string stringUrl = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&cnt=1&APPID={1}", cityName, apiId);

            using (WebClient client = new WebClient())
            {

                try
                {
                    var url = new Uri(stringUrl);
                    string json = await client.DownloadStringTaskAsync(url);

                    // Converting to OBJECT from JSON string.
                    ForecastModel weatherInfo = JsonConvert.DeserializeObject<ForecastModel>(json);

                    // Special VIEWMODEL design to send only required fields not all fields which received from www.openweathermap.org api
                    ForecastResult weatherResult = new ForecastResult();

                    weatherResult.City = weatherInfo.Name;
                    weatherResult.Description = weatherInfo.Weather[0].Description;
                    weatherResult.Humidity = Convert.ToString(weatherInfo.Main.Humidity);
                    weatherResult.Temp = Convert.ToString(weatherInfo.Main.Temp);
                    weatherResult.TempFeelsLike = Convert.ToString(weatherInfo.Main.Feels_like);
                    weatherResult.TempMax = Convert.ToString(weatherInfo.Main.Temp_max);
                    weatherResult.TempMin = Convert.ToString(weatherInfo.Main.Temp_min);
                    weatherResult.WeatherIcon = weatherInfo.Weather[0].Icon;

                    return weatherResult;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
