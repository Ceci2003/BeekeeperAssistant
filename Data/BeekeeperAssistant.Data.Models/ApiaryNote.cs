namespace BeekeeperAssistant.Data.Models
{
    using BeekeeperAssistant.Data.Common.Models;

    public class ApiaryNote : BaseDeletableModel<int>
    {
        public int ApiaryId { get; set; }

        public virtual Apiary Apiary { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Color { get; set; }

        public string ModifiendById { get; set; }

        public virtual ApplicationUser ModifiendBy { get; set; }
    }
}
