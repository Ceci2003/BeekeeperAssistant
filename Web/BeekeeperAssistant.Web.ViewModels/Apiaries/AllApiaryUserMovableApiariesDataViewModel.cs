namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AllApiaryUserMovableApiariesDataViewModel : IMapFrom<Apiary>, IMapFrom<ApiaryDataModel>
    {
        public string Number { get; set; }

        public string Name { get; set; }

        public bool IsBookMarked { get; set; }

        public ApiaryType ApiaryType { get; set; }

        public string Adress { get; set; }

        public int Id { get; set; }

        public int BeehivesCount { get; set; }

        public bool IsRegistered { get; set; }

        public bool IsClosed { get; set; }

        public DateTime OpeningDate { get; set; }

        public DateTime ClosingDate { get; set; }
    }
}
