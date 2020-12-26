namespace BeekeeperAssistant.Web.ViewModels.Queens
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using BeekeeperAssistant.Data.Models;

    public class CreateQueenInputModel
    {

        public string Number { get; set; }


        public DateTime FertilizationDate { get; set; }

        [Required]
        public DateTime GivingDate { get; set; }

        public QueenType QueenType { get; set; }

        public string Origin { get; set; }

        public string ChangeReason { get; set; }

        public string HygenicHabits { get; set; }

        public string Temparament { get; set; }

        [Required]
        public QueenColor QueenColor { get; set; }

        [Required]
        public QueenBreed QueenBreed { get; set; }

    }
}
