namespace BeekeeperAssistant.Data.Models
{
    using BeekeeperAssistant.Data.Common.Models;

    public class SystemNotification : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Version { get; set; }

        public string ImageUrl { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }
    }
}
