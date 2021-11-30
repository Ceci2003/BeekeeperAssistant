namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class HarvestedBeehive
    {
        [ForeignKey(nameof(Beehive))]
        public int BeehiveId { get; set; }

        public virtual Beehive Beehive { get; set; }

        [ForeignKey(nameof(Harvest))]
        public int HarvestId { get; set; }

        public virtual Harvest Harvest { get; set; }
    }
}
