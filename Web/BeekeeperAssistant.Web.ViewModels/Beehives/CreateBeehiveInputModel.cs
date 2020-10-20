namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using BeekeeperAssistant.Data.Models;

    public class CreateBeehiveInputModel
    {
        public int ApiaryId { get; set; }

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

        public bool HasDevice { get; set; }

        public bool HasPolenCatcher { get; set; }

        public bool HasPropolisCatcher { get; set; }
    }
}
