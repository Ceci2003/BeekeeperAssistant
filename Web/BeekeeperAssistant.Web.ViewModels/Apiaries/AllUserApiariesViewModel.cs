using BeekeeperAssistant.Data.Models;
using BeekeeperAssistant.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    public class AllUserApiariesViewModel : IMapFrom<UsersApiaries>
    {

        public string ApiaryNumber { get; set; }

        public string ApiaryName { get; set; }

        public ApiaryType ApiaryApiaryType { get; set; }

        public string ApiaryAdress { get; set; }

        public Apiary Apiary { get; set; }

    }
}
