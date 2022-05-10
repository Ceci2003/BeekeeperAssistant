namespace BeekeeperAssistant.Web.ViewModels.Queens
{
    using BeekeeperAssistant.Data.Filters.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AllQueenViewModel
    {
        public FilterModel AllQueensFilterModel { get; set; }

        public IEnumerable<QueenDataModel> AllQueens { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
