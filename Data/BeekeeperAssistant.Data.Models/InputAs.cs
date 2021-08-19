namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum InputAs
    {
        [Display(Name = "На кошер")]
        PerHive = 1,
        [Display(Name = "Общо")]
        Total = 2,
    }
}
