namespace BeekeeperAssistant.Web.ViewModels.BeehiveDiaries
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using Ganss.XSS;

    public class ByBeehiveIdBeehiveDiaryViewModel : IMapFrom<BeehiveDiary>
    {
        public int BeehiveId { get; set; }

        public int BeehiveNumber { get; set; }

        public string BeehiveApiaryNumber { get; set; }

        public string BeehiveApiaryName { get; set; }

        public int BeehiveApiaryId { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

    }
}
