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
        [Display(Name = "Слънчогледов")]
        Sunflower = 2,
        [Display(Name = "От детелина")]
        Clover = 3,
        [Display(Name = "От люцерна")]
        Alfalfa = 4,
        [Display(Name = "Друг")]
        Other = 5,
    }
}
