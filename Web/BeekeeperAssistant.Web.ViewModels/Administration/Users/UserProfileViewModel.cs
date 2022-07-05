namespace BeekeeperAssistant.Web.ViewModels.Administration.Users
{
    using System;
    using System.Collections.Generic;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class UserProfileViewModel : IMapFrom<ApplicationUser>
    {
        public ApplicationUser User { get; set; }
    }
}
