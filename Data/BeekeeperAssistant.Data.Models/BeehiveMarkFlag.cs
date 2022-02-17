namespace BeekeeperAssistant.Data.Models
{
    using BeekeeperAssistant.Data.Common.Models;

    public class BeehiveMarkFlag : BaseDeletableModel<int>
    {
        public int BeehiveId { get; set; }

        public Beehive Beehive { get; set; }

        public string Content { get; set; }

        public MarkFlagType FlagType { get; set; }
    }
}
