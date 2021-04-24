namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum QueenType
    {
        [Display(Name = "Произведена")]
        Produced = 1,
        [Display(Name = "няма")]
        None = 2,
        [Display(Name = "Роева")]
        SwarmingMother = 3,
        [Display(Name = "Самосмяна")]
        SelfReplacement = 4,
        [Display(Name = "Свищева")]
        Fistulous = 5,
    }
}
