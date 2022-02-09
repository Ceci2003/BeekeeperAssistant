using System.ComponentModel.DataAnnotations;

namespace BeekeeperAssistant.Data.Models
{
    public enum MarkFlagType
    {
        [Display(Name = "Червен")]
        Red = 1,
        [Display(Name = "Ореанжев")]
        Orange = 2,
        [Display(Name = "Зелен")]
        Green = 3,
    }
}
