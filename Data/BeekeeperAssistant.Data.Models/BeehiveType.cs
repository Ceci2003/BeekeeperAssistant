namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum BeehiveType
    {
        [Display(Name = "Пчелно семейство")]
        BeeFamily = 1,
        [Display(Name = "Рояк")]
        Swarm = 2,
        [Display(Name = "Кошер")]
        Beehive = 3,
        [Display(Name = "Отводка")]
        ForPropagation = 4,
        [Display(Name = "Друг")]
        Other = 5,
    }
}
