namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BeehiveHelper
    {
        public int BeehiveId { get; set; }

        public Beehive Beehive { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Access Access { get; set; }
    }
}
