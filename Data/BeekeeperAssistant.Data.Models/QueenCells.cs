namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum QueenCells
    {
        [Display(Name = "Няма")]
        None = 1,
        [Display(Name = "Роеви")]
        Swarm = 2,
        [Display(Name = "Самосмяна")]
        Surersedure = 3,
        [Display(Name = "Свищеви")]
        Emergency = 4,
    }
}
