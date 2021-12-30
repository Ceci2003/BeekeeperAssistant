namespace BeekeeperAssistant.Data.Models
{
    using BeekeeperAssistant.Data.Common.Models;

    public class BeehiveNote : BaseDeletableModel<int>
    {
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public int BeehiveId { get; set; }

        public Beehive Beehive { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Color { get; set; }
    }
}
