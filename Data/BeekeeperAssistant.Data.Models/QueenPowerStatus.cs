namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum QueenPowerStatus
    {
        [Display(Name = "Силна")]
        Strong = 1,
        [Display(Name = "Слаба")]
        Weak = 2,
    }
}
