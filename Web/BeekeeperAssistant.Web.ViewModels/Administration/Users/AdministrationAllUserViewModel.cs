namespace BeekeeperAssistant.Web.ViewModels.Administration.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AdministrationAllUserViewModel
    {
        public IEnumerable<UserViewModel> AllUsers { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
