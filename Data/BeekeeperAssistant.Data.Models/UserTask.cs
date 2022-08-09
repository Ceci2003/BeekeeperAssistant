namespace BeekeeperAssistant.Data.Models
{
    using System;

    using BeekeeperAssistant.Data.Common.Models;

    public class UserTask : BaseDeletableModel<int>
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Color { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
