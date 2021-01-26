namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum QueenColor
    {
        [Display(Name = "Бял")]
        White = 1,
        [Display(Name = "Жълт")]
        Yellow = 2,
        [Display(Name = "Червен")]
        Red = 3,
        [Display(Name = "Зелен")]
        Green = 4,
        [Display(Name = "Син")]
        Blue = 5,
    }
}