namespace BeekeeperAssistant.Web.ViewModels.ApiaryHelpers
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class ApiaryHelperApiaryDataViewModel : IMapFrom<ApiaryHelper>
    {
        public string ApiaryNumber { get; set; }

        public string ApiaryName { get; set; }

        public bool ApiaryIsBookMarked { get; set; }

        public ApiaryType ApiaryApiaryType { get; set; }

        public string ApiaryCreatorUserName { get; set; }

        public string ApiaryAdress { get; set; }

        public int ApiaryId { get; set; }

        public int ApiaryBeehivesCount { get; set; }

        public Access Access { get; set; }
    }
}
