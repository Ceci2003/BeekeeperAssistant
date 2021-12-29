namespace BeekeeperAssistant.Web.ViewModels.QueenHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AllQueenHelperViewModel
    {
        public IEnumerable<QueenHelperViewModel> AllHelpers { get; set; }

        public int BeehiveId { get; set; }

        public int BeehiveNumber { get; set; }

        public string ApiaryNumber { get; set; }

        public int QueenId { get; set; }
    }
}
