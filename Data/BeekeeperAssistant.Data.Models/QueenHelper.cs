using System;
using System.Collections.Generic;
using System.Text;

namespace BeekeeperAssistant.Data.Models
{
    public class QueenHelper
    {
        public int QueenId { get; set; }

        public Queen Queen { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Access Access { get; set; }
    }
}
