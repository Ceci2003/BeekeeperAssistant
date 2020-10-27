using BeekeeperAssistant.Data.Models;
using BeekeeperAssistant.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    public class ApiaryDataViewModel : IMapFrom<Apiary>
    {
        public string Number { get; set; }

        public ApiaryType ApiaryType { get; set; }

        public string Adress { get; set; }

        public string Name { get; set; }

    }
}
