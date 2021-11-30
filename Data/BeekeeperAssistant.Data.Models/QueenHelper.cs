namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class QueenHelper
    {
        public int QueenId { get; set; }

        public virtual Queen Queen { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Access Access { get; set; }
    }
}
