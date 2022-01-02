namespace BeekeeperAssistant.Data.Models
{
    using BeekeeperAssistant.Data.Common.Models;

    public class BeehiveNote : BaseDeletableModel<int>
    {
        public int BeehiveId { get; set; }

        public virtual Beehive Beehive { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Color { get; set; }

        public string ModifiendById { get; set; }

        public virtual ApplicationUser ModifiendBy { get; set; }
    }
}
