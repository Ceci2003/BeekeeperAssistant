namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class EditApiaryInputModel : IMapFrom<Apiary>
    {
        [Required]
        public string Number { get; set; }

        public string Name { get; set; }

        [Required]
        public ApiaryType ApiaryType { get; set; }

        public string Adress { get; set; }

    }
}
