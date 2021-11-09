using BeekeeperAssistant.Data.Models;
using BeekeeperAssistant.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Services.Data.Models
{
    public class TreatmentDataExcelExportModel : IMapFrom<Treatment>
    {
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public DateTime DateOfTreatment { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public string Disease { get; set; }

        public string Medication { get; set; }

        public InputAs InputAs { get; set; }

        public double Quantity { get; set; }

        public Dose Dose { get; set; }
    }
}
