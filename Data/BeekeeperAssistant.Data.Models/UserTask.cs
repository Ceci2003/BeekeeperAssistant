namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Common.Models;

    public class UserTask : BaseDeletableModel<int>
    {
        public string Title { get; set; }

#nullable enable
        public string? Text { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsCompleted { get; set; }

#nullable disable
    }
}
