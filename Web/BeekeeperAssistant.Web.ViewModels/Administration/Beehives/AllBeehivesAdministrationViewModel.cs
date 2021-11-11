namespace BeekeeperAssistant.Web.ViewModels.Administration.Beehives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AllBeehivesAdministrationViewModel
    {
        public IEnumerable<BeehivesAdministrationViewModel> AllBeehives { get; set; }
    }
}
