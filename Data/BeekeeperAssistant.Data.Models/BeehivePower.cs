using System.ComponentModel.DataAnnotations;

namespace BeekeeperAssistant.Data.Models
{
    public enum BeehivePower
    {
        [Display(Name = "Слаб")]
        Weak = 1,
        [Display(Name = "Среден")]
        Medium = 2,
        [Display(Name = "Силен")]
        Strong = 3,
    }
}
