namespace BeekeeperAssistant.Web.ViewModels.BeehiveHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllBeehiveHelperViewModel
    {
        public IEnumerable<BeehiveHelperViewModel> AllHelpers { get; set; }

        public int BeehiveId { get; set; }

        public int BeehiveNumber { get; set; }

        public string ApiaryNumber { get; set; }
    }
}
