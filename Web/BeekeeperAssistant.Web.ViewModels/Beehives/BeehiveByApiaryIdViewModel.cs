namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using System;

    public class BeehiveByApiaryIdViewModel : IMapFrom<Beehive>
    {
        public int Id { get; set; }

        public int ApiaryId { get; set; }

        public string ApiaryNumber { get; set; }

        public int? QueenId { get; set; }

        public DateTime? QueenGivingDate { get; set; }

        public QueenColor? QueenColor { get; set; }

        public int Number { get; set; }

        public bool IsBookMarked { get; set; }

        public bool HasDevice { get; set; }

        public Access BeehiveAccess { get; set; }

        public BeehiveType BeehiveType { get; set; }

        public BeehiveSystem BeehiveSystem { get; set; }

         public BeehivePower BeehivePower { get; set; }
    }
}