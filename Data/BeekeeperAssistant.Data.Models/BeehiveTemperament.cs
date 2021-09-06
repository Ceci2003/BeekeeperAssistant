namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum BeehiveTemperament
    {
        [Display(Name = "Спокоен")]
        Calm = 1,
        [Display(Name = "Нервен")]
        Nervous = 2,
        [Display(Name = "Агресивен")]
        Aggressive = 3,
    }
}
