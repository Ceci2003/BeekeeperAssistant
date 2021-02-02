namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;

    public class CreateBeehiveInputModel
    {
        [Required]
        public int Number { get; set; }

        [Required]
        public BeehiveSystem BeehiveSystem { get; set; }

        [Required]
        public BeehiveType BeehiveType { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public BeehivePower BeehivePower { get; set; }

        public IEnumerable<SelectListOptionApiaryViewModel> AllApiaries { get; set; }

        public int ApiaryId { get; set; }

        public bool HasDevice { get; set; }

        public bool HasPolenCatcher { get; set; }

        public bool HasPropolisCatcher { get; set; }
    }
}
