namespace BeekeeperAssistant.Services.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class HarvestDataExcelExportModel : IMapFrom<Harvest>
    {

        public string HarvestName { get; set; }

        public DateTime DateOfHarves { get; set; }

        public string Note { get; set; }

        public HarvestProductType HarvestProductType { get; set; }

        public HoneyType HoneyType { get; set; }

        public double Quantity { get; set; }

        public Unit Unit { get; set; }
    }
}
