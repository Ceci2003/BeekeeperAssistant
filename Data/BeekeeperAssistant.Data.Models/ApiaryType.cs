namespace BeekeeperAssistant.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public enum ApiaryType
    {
        Основен = 1,
        Стационарен = 2,
        Подвижен = 3,
        Помощен = 4,
        [Display(Name = "за майко производство")]
        заМйкоПроизводство = 5,
        [Display(Name = "за отводки")]
        заОтводки = 6,
    }
}
