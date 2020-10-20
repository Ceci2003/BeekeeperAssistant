namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class UserApiaryViewModel : IMapFrom<UsersApiaries>
    {
        public string ApiaryNumber { get; set; }

        public string ApiaryName { get; set; }

        public ApiaryType ApiaryApiaryType { get; set; }

        public string ApiaryAdress { get; set; }

        public Apiary Apiary { get; set; }
    }
}
