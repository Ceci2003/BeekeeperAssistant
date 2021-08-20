namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ApiaryHelper
    {
        public int ApiaryId { get; set; }

        public Apiary Apiary { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public bool CanRead { get; set; }

        public bool CanWrite { get; set; }
    }
}
