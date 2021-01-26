namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public enum ApiaryType
    {
        [Display(Name = "Основен")]
        Basic = 1,
        [Display(Name = "Стационарен")]
        Stationary = 2,
        [Display(Name = "Подвижен")]
        Mobile = 3,
        [Display(Name = "Помощен")]
        Supporting = 4,
        [Display(Name = "за майко производство")]
        ForTheProductionOfMothers = 5,
        [Display(Name = "за отводки")]
        ForPropagation = 6,
        [Display(Name = "друг")]
        Other = 5,
    }
}
