namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum Activity
    {
        [Display(Name = "Слаба")]
        Low = 1,
        [Display(Name = "Средна")]
        Average = 2,
        [Display(Name = "Силна")]
        High = 3,
    }
}
