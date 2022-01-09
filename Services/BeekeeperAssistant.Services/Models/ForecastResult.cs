namespace BeekeeperAssistant.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ForecastResult
    {
        public string City { get; set; }

        public string Description { get; set; }

        public string Humidity { get; set; }

        public string TempFeelsLike { get; set; }

        public string Temp { get; set; }

        public string TempMax { get; set; }

        public string TempMin { get; set; }

        public string WeatherIcon { get; set; }

        public int StatusCode { get; set; }

        public string StatusMesaage { get; set; }
    }
}
