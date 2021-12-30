namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System.Collections.Generic;

    using BeekeeperAssistant.Web.ViewModels.ApiaryHelpers;

    public class AllApiaryUserHelperApiariesViewModel
    {
        public IEnumerable<AllApiaryUserHelperApiariesDataViewModel> AllUserHelperApiaries { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
