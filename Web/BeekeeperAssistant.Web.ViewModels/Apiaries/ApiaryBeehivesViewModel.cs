namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System.Collections.Generic;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Web.ViewModels.Beehives;

    public class ApiaryBeehivesViewModel
    {
        public ApiaryDataViewModel Apiary { get; set; }

        public IEnumerable<BeehiveViewModel> Beehives { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
