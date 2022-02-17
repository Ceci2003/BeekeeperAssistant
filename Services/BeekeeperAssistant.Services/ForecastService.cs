namespace BeekeeperAssistant.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Services.Messaging;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    public class ForecastService : IForecastService
    {
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;

        public ForecastService(
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            this.emailSender = emailSender;
            this.configuration = configuration;
        }

        public async Task<ForecastResult> GetApiaryCurrentWeatherByCityName(string cityName, string apiId)
        {
            string stringUrl = $"http://api.openweathermap.org/data/2.5/weather?q={cityName}&units=metric&cnt=1&APPID={apiId}";

            using (WebClient client = new WebClient())
            {
                try
                {
                    var url = new Uri(stringUrl);

                    try
                    {
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
                    catch (WebException ex)
                    {
                        var statusCodeInHttpCode = ((HttpWebResponse)ex.Response).StatusCode;
                        ForecastResult forecastResult = new ForecastResult();

                        if (statusCodeInHttpCode != HttpStatusCode.NotFound)
                        {
                            // !TODO make send email to administrators when catch exeption status code different from 404
                            // Also send email to multiple users
                            await this.emailSender.SendEmailAsync(
                                  this.configuration["SendGrid:RecipientEmail"],
                                  GlobalConstants.SystemName,
                                  this.configuration["SendGrid:RecipientEmail"],
                                  $"System Exeption - {(int)statusCodeInHttpCode}",
                                  $"System exeption were cathed {(int)statusCodeInHttpCode} - {statusCodeInHttpCode}");
                        }

                        return null;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public async Task<ForecastResult> GetApiaryCurrentWeatherByCityPostcode(string postcode, string apiId)
        {
            string stringUrl = $"http://api.openweathermap.org/data/2.5/weather?zip={postcode},BG&units=metric&cnt=1&appid={apiId}";

            using (WebClient client = new WebClient())
            {
                try
                {
                    var url = new Uri(stringUrl);

                    try
                    {
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
                    catch (WebException ex)
                    {
                        var statusCodeInHttpCode = ((HttpWebResponse)ex.Response).StatusCode;
                        ForecastResult forecastResult = new ForecastResult();

                        if (statusCodeInHttpCode != HttpStatusCode.NotFound)
                        {
                            // !TODO make send email to administrators when catch exeption status code different from 404
                            // Also send email to multiple users
                            await this.emailSender.SendEmailAsync(
                                  this.configuration["SendGrid:RecipientEmail"],
                                  GlobalConstants.SystemName,
                                  this.configuration["SendGrid:RecipientEmail"],
                                  $"System Exeption - {(int)statusCodeInHttpCode}",
                                  $"System exeption were cathed {(int)statusCodeInHttpCode} - {statusCodeInHttpCode}");
                        }

                        return null;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public bool ValidateCityPostcode(string postcode, string apiId)
        {
            string stringUrl = $"http://api.openweathermap.org/data/2.5/weather?zip={postcode},BG&appid={apiId}";

            using (WebClient client = new WebClient())
            {
                try
                {
                    var url = new Uri(stringUrl);

                    try
                    {
                        string json = client.DownloadString(url);
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
