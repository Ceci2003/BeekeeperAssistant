namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using AutoMapper;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;

    public class BeehiveDataModel : IMapFrom<Beehive>, IMapFrom<BeehiveDataModel>
    {
        public int Id { get; set; }

        public Apiary Apiary { get; set; }

        public string ApiaryNumber { get; set; }

        public int Number { get; set; }

        public BeehivePower BeehivePower { get; set; }

        public BeehiveSystem BeehiveSystem { get; set; }

        public BeehiveType BeehiveType { get; set; }

        public bool IsBookMarked { get; set; }

        public bool HasDevice { get; set; }

        public bool IsItMovable { get; set; }

        public Access BeehiveAccess { get; set; }

        public string CreatorId { get; set; }

        public MarkFlagType? MarkFlagType { get; set; }
    }
}
