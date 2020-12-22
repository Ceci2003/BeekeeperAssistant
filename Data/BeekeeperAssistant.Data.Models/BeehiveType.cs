namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum BeehiveType
    {
        [Display(Name = "Пчелно семейство")]
        ПчелноСемейство = 1,
        [Display(Name = "рояк")]
        swarm = 2,
        [Display(Name = "кошер")]
        beehive = 3,
        [Display(Name = "отводка")]
        reproductive = 4,
        [Display(Name = "друг")]
        Other = 5,
    }
}
