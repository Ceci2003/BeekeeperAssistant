namespace BeekeeperAssistant.Web.ViewModels.ApiaryHelpers
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class ApiaryHelperViewModel : IMapFrom<ApiaryHelper>
    {
        public string UserUserName { get; set; }

        public Access Access { get; set; }
    }
}