namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllApiariesViewModel
    {
       public IEnumerable<ApiaryViewModel> AllUserApiaries { get; set; }

       public int CurrentPage { get; set; }

       public int PagesCount { get; set; }
    }
}
