namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum QueenCells
    {
        [Display(Name = "Няма")]
        None = 1,
        [Display(Name = "Свищеви")]
        Swarm = 2,
        [Display(Name = "")]
        Surersedure = 3,
        [Display(Name = "")]
        Emergency = 4,
    }
}
