namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class TreatedBeehive
    {
        [ForeignKey(nameof(Beehive))]
        public int BeehiveId { get; set; }

        public Beehive Beehive { get; set; }

        [ForeignKey(nameof(Treatment))]
        public int TreatmentId { get; set; }

        public Treatment Treatment { get; set; }
    }
}
