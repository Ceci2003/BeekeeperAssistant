namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum StoragePower
    {
        [Display(Name = "Слаби")]
        Low = 1,
        [Display(Name = "Достатъчни")]
        Enought = 2,
    }
}
