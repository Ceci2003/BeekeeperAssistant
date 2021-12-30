namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    //{area?}{action}{controller}{FieldName?...Data}...ViewModel

    public class AllApiaryUserApiariesViewModel
    {
        public IEnumerable<AllApiaryUserApiariesDataViewModel> AllUserApiaries { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
