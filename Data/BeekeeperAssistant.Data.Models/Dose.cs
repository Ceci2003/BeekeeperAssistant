namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum Dose
    {
        [Display(Name = "ленти")]
        Strips = 1,
        [Display(Name = "капки")]
        Drops = 2,
        [Display(Name = "г.")]
        Grams = 3,
        [Display(Name = "кг.")]
        Kilograms = 4,
        [Display(Name = "мл.")]
        Millilitres = 5,
        [Display(Name = "л.")]
        Litres = 6,
    }
}
