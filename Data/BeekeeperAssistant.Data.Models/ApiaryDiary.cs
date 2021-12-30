namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Common.Models;

    public class ApiaryDiary : BaseDeletableModel<int>
    {
        public string Content { get; set; }

        public int ApiaryId { get; set; }

        public virtual Apiary Apiary { get; set; }

        public string ModifiendById { get; set; }

        public virtual ApplicationUser ModifiendBy { get; set; }
    }
}
