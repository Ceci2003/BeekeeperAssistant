namespace BeekeeperAssistant.Web.ViewModels.BeehiveHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllBeehiveHelpersViewModel 
    {
        public IEnumerable<BeehiveHelperViewModel> AllHelpers { get; set; }

    }
}
