namespace BeekeeperAssistant.Web.ViewModels.Inspection
{
    using System.Collections.Generic;

    public class AllInspectionsViewModel
    {
        public IEnumerable<InspectionDataViewModel> AllInspections { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
