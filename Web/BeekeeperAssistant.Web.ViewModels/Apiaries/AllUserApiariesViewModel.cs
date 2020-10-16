namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllUserApiariesViewModel : IMapFrom<UsersApiaries>
    {
       public IEnumerable<UserApiaryViewModel> AllUserApiaries { get; set; }
    }
}
