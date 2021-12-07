namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ApiaryHelper
    {
        public int ApiaryId { get; set; }

        public virtual Apiary Apiary { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Access Access { get; set; }
    }
}
