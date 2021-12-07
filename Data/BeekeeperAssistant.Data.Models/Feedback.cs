using BeekeeperAssistant.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeekeeperAssistant.Data.Models
{
    public class Feedback : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public FeedbackType FeedbackType { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
