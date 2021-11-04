namespace BeekeeperAssistant.Web.ViewModels.Treatments
{
    using System.Collections.Generic;

    public class AllTreatementsViewModel
    {
        public IEnumerable<TreatmentDataViewModel> AllTreatements { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
