namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum HarvestProductType
    {
        [Display(Name = "мед")]
        Honey = 1,
        [Display(Name = "прашец")]
        Pollen = 2,
        [Display(Name = "восък")]
        Wax = 3,
        [Display(Name = "прополис")]
        Propolis = 4,
        [Display(Name = "млечице")]
        RoyalJelly = 5,
        [Display(Name = "отрова")]
        BeeVenom = 6,
    }
}
