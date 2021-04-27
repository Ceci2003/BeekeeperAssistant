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
            string stringUrl = $"http://api.openweathermap.org/data/2.5/weather?q={cityName}&units=metric&cnt=1&APPID={apiId}";

            using (WebClient client = new WebClient())
            {
                try
                {
                    var url = new Uri(stringUrl);
                    string json = await client.DownloadStringTaskAsync(url);

                    ForecastModel weatherInfo = JsonConvert.DeserializeObject<ForecastModel>(json);

                    ForecastResult weatherResult = new ForecastResult
                    {
                        City = weatherInfo.Name,
                        Description = weatherInfo.Weather[0].Description,
                        Humidity = weatherInfo.Main.Humidity.ToString(),
                        Temp = weatherInfo.Main.Temp.ToString(),
                        TempFeelsLike = weatherInfo.Main.Feels_like.ToString(),
                        TempMax = weatherInfo.Main.Temp_max.ToString(),
                        TempMin = weatherInfo.Main.Temp_min.ToString(),
                        WeatherIcon = weatherInfo.Weather[0].Icon,
                    };

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
