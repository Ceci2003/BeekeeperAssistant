namespace BeekeeperAssistant.Web.ViewModels.Treatments
{
    using System.Collections.Generic;

    using BeekeeperAssistant.Data.Models;

    public class AllByBeehiveIdTreatementViewModel
    {
        public IEnumerable<TreatmentDataViewModel> AllTreatements { get; set; }

        public Access BeehiveAccess { get; set; }

        public int BeehiveId { get; set; }

        public int BeehiveNumber { get; set; }

        public int ApiaryId { get; set; }

        public string ApiaryNumber { get; set; }

        public string ApiaryName { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
