namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Common.Models;

    public class BeehiveDiary : BaseDeletableModel<int>
    {
        public string Content { get; set; }

        public int BeehiveId { get; set; }

        public virtual Beehive Beehive { get; set; }

        public string ModifiendById { get; set; }

        public virtual ApplicationUser ModifiendBy { get; set; }
    }
}
