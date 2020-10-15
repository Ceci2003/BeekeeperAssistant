using BeekeeperAssistant.Data.Models;
using BeekeeperAssistant.Services.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    public class CreateApiaryInputModel
    {
        [Required]
        public string Number { get; set; }

        public string Name { get; set; }

        [Required]
        public ApiaryType ApiaryType { get; set; }

        public string Adress { get; set; }
    }
}
