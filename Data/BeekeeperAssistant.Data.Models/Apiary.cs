﻿namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Common.Models;

    public class Apiary : BaseDeletableModel<int>
    {
        public int Number { get; set; }

        public string Name { get; set; }

        public ApiaryType ApiaryType { get; set; }

        public string Location { get; set; }
    }
}
