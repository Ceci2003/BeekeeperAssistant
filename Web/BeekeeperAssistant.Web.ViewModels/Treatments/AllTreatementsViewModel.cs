namespace BeekeeperAssistant.Web.ViewModels.Treatments
{
    using BeekeeperAssistant.Data.Models;
    using System.Collections.Generic;

    public class AllTreatementsViewModel
    {
        public IEnumerable<TreatmentDataViewModel> AllTreatements { get; set; }

        public Access BeehiveAccess { get; set; }

        public int BeehiveId { get; set; }

        public int BeehiveNumber { get; set; }

        public string ApiaryNumber { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
