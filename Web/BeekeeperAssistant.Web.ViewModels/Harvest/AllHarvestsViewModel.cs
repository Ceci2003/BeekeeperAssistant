using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Web.ViewModels.Harvest
{
    public class AllHarvestsViewModel
    {
        public IEnumerable<HarvestDatavVewModel> AllHarvests { get; set; }
    }
}
