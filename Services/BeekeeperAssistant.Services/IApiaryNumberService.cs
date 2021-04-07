namespace BeekeeperAssistant.Services
{
    using System.Collections.Generic;
    using System.Text;

    public interface IApiaryNumberService
    {
        string GetCityCode(string number);

        string GetFarmNumber(string number);

        string CreateApiaryNumber(string cityCode, string farmNumber);
    }
}
