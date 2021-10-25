namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class ApiaryViewModel : IMapFrom<Apiary>
    {
        public string Number { get; set; }

        public string Name { get; set; }

        public bool IsBookMarked { get; set; }

        public ApiaryType ApiaryType { get; set; }

        public string CreatorId { get; set; }

        public string Adress { get; set; }

        public int Id { get; set; }

        public int BeehivesCount { get; set; }
    }
}
