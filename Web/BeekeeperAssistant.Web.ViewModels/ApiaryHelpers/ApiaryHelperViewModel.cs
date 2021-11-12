namespace BeekeeperAssistant.Web.ViewModels.ApiaryHelpers
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class ApiaryHelperViewModel : IMapFrom<ApiaryHelper>
    {
        public string UserUserName { get; set; }

        public string UserId { get; set; }

        public int ApiaryId { get; set; }

        public Access Access { get; set; }
    }
}
