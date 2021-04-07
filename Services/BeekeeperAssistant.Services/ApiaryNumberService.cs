namespace BeekeeperAssistant.Services
{
    using System;

    public class ApiaryNumberService : IApiaryNumberService
    {
        public string CreateApiaryNumber(string cityCode, string farmNumber)
        {
            var apiaryNumber = $"{cityCode}-{farmNumber}";
            return apiaryNumber;
        }

        public string GetCityCode(string apiaryNumber)
        {
            var cityNumber = apiaryNumber.Split("-", 2, StringSplitOptions.RemoveEmptyEntries)[0];

            return cityNumber;
        }

        public string GetFarmNumber(string apiaryNumber)
        {
            var farmNumber = apiaryNumber.Split("-", 2, StringSplitOptions.RemoveEmptyEntries)[1];

            return farmNumber;
        }


    }
}
