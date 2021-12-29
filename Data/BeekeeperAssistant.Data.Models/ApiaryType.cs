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
        [Display(Name = "Временен / Подвижен")]
        Mobile = 3,
        [Display(Name = "Помощен")]
        Supporting = 4,
        [Display(Name = "За майко производство")]
        ForTheProductionOfMothers = 5,
        [Display(Name = "За отводки")]
        ForPropagation = 6,
        [Display(Name = "Друг")]
        Other = 7,
    }
}
