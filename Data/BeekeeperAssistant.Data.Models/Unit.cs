namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum Unit
    {
        [Display(Name = "кг")]
        Kilograms = 1,
        [Display(Name = "гр")]
        Grams = 2,
        [Display(Name = "мг")]
        Milligrams = 3,
        [Display(Name = "л")]
        Litres = 4,
        [Display(Name = "мл")]
        Millilitres = 5,
    }
}
