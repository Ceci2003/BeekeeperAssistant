﻿namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;

    public class BeehiveDataModel : IMapFrom<Beehive>
    {
        public int Id { get; set; }

        public ApiaryDataModel Apiary { get; set; }

        public int Number { get; set; }

        public BeehivePower BeehivePower { get; set; }

        public BeehiveSystem BeehiveSystem { get; set; }

        public BeehiveType BeehiveType { get; set; }

        public bool IsBookMarked { get; set; }

        public bool HasDevice { get; set; }

        public bool IsItMovable { get; set; }

        public Access BeehiveAccess { get; set; }

        public string CreatorId { get; set; }
    }
}