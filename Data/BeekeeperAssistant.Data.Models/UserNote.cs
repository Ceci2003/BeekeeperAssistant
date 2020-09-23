namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Common.Models;

    public class UserNote : BaseDeletableModel<int>
    {
        public string Title { get; set; }

#nullable enable
        public string? Text { get; set; }

        public virtual ICollection<UserNotes> UserNotes { get; set; }
    }
}
