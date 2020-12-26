namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum QueenType
    {
        [Display(Name = "произведена")]
        Produced = 1,
        [Display(Name = "няма")]
        None = 2,
        [Display(Name = "роева")]
        SwarmingMother = 3,
        [Display(Name = "самосмяна")]
        SelfReplacement = 4,
        [Display(Name = "свищева")]
        Fistulous = 5,
    }
}
