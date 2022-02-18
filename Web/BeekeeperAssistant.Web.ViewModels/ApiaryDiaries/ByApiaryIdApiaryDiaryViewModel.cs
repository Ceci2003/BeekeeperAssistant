namespace BeekeeperAssistant.Web.ViewModels.ApiaryDiaries
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using Ganss.XSS;

    public class ByApiaryIdApiaryDiaryViewModel : IMapFrom<ApiaryDiary>
    {
        public int ApiaryId { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public string ApiaryNumber { get; set; }

        public string ApiaryName { get; set; }

        public ApiaryType ApiaryApiaryType { get; set; }
    }
}
