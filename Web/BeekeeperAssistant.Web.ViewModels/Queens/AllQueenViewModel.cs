namespace BeekeeperAssistant.Web.ViewModels.Queens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AllQueenViewModel
    {
        public IEnumerable<QueenViewModel> AllQueens { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
