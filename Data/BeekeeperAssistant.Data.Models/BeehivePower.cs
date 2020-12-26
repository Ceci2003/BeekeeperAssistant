namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public enum BeehivePower
    {
        [Display(Name = "Слаб")]
        [Description("Слаб")]
        Weak = 1,
        [Display(Name = "Среден")]
        [Description("Среден")]
        Medium = 2,
        [Display(Name = "Силен")]
        [Description("Силен")]
        Strong = 3,
    }
}
