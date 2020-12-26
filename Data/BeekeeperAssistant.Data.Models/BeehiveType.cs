namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum BeehiveType
    {
        [Display(Name = "Пчелно семейство")]
        BeeFamily = 1,
        [Display(Name = "рояк")]
        Swarm = 2,
        [Display(Name = "кошер")]
        Beehive = 3,
        [Display(Name = "отводка")]
        ForPropagation = 4,
        [Display(Name = "друг")]
        Other = 5,
    }
}
