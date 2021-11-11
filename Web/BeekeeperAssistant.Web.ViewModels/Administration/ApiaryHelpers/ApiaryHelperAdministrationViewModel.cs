namespace BeekeeperAssistant.Web.ViewModels.Administration.ApiaryHelpers
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class ApiaryHelperAdministrationViewModel : IMapFrom<ApiaryHelper>
    {
        public string UserId { get; set; }

        public int ApiaryId { get; set; }

        public string UserUsername { get; set; }

        public string ApiaryNumber { get; set; }

        public string ApiaryName { get; set; }

        public string ApiaryAdress { get; set; }
    }
}