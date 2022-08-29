namespace BeekeeperAssistant.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;

    public class AdministrationAllUserBoxViewModel
    {
        public IEnumerable<UserBoxViewModel> AllUsers { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
