namespace BeekeeperAssistant.Data.Models
{
    using BeekeeperAssistant.Data.Common.Models;

    public class TemporaryApiaryBeehive : BaseDeletableModel<int>
    {
        public int ApiaryId { get; set; }

        public Apiary Apiary { get; set; }

        public int BeehiveId { get; set; }

        public Beehive Beehive { get; set; }
    }
}
