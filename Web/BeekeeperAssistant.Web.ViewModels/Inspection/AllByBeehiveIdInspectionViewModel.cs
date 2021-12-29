namespace BeekeeperAssistant.Web.ViewModels.Inspection
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using System.Collections.Generic;

    public class AllByBeehiveIdInspectionViewModel
    {
        public IEnumerable<InspectionDataViewModel> AllInspections { get; set; }

        public int BeehiveId { get; set; }

        public int BeehiveNumber { get; set; }

        public string ApiaryNumber { get; set; }

        public Access BeehiveAccess { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
