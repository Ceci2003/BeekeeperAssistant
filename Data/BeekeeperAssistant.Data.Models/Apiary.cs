﻿namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Common.Models;

    public class Apiary : BaseDeletableModel<int>
    {
        public Apiary()
        {
            this.Apiaries = new HashSet<Apiary>();
        }

        public string Number { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

#nullable enable
        public string? Name { get; set; }

        public ApiaryType ApiaryType { get; set; }

#nullable enable
        public string? Adress { get; set; }

#nullable disable

        public virtual ICollection<Apiary> Apiaries { get; set; }

        public virtual ICollection<Beehive> Beehives { get; set; }
    }
}