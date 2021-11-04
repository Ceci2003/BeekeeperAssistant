namespace BeekeeperAssistant.Services.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class ApiaryDataExcelExportModel : IMapFrom<Apiary>
    {
        public string Number { get; set; }

        public ApiaryType ApiaryType { get; set; }

        public string Adress { get; set; }

        public string Name { get; set; }
    }
}
