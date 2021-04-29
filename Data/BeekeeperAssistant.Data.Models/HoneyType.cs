namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public enum HoneyType
    {
        [Display(Name = "Акация")]
        Acacia = 0,
        [Display(Name = "Билков")]
        Wildflower = 1,
        [Display(Name = "Слънчоглед")]
        Sunflower = 2,
        [Display(Name = "Детелина")]
        Clover = 3,
        [Display(Name = "Люцерна")]
        Alfalfa = 4,
        [Display(Name = "Друг")]
        Other = 5,
    }
}
