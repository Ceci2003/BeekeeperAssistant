namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class ByMovableApiaryIdBeehiveViewModel : IMapFrom<TemporaryApiaryBeehive>
    {
        public int BeehiveId { get; set; }

        public int BeehiveApiaryId { get; set; }

        public string BeehiveApiaryNumber { get; set; }

        public string BeehiveApiaryName { get; set; }

        public int BeehiveNumber { get; set; }

        public bool BeehiveIsBookMarked { get; set; }

        public bool BeehiveHasDevice { get; set; }

        public int? BeehiveQueenId { get; set; }

        public DateTime? BeehiveQueenGivingDate { get; set; }

        public QueenColor? BeehiveQueenColor { get; set; }

        public Access BeehiveBeehiveAccess { get; set; }

        public BeehiveType BeehiveBeehiveType { get; set; }

        public BeehiveSystem BeehiveBeehiveSystem { get; set; }

        public BeehivePower BeehiveBeehivePower { get; set; }

        public MarkFlagType? MarkFlagType { get; set; }
    }
}
