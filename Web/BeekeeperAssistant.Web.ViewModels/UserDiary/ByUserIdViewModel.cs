namespace BeekeeperAssistant.Web.ViewModels.UserDiary
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using Ganss.XSS;

    public class ByUserIdViewModel : IMapFrom<UserDiary>
    {
        public string UserId { get; set; }

        public int UserDiaryId { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);
    }
}
