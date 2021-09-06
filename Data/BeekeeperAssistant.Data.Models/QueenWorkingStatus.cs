namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum QueenWorkingStatus
    {
        [Display(Name = "Работи")]
        Working = 1,
        [Display(Name = "Не работи")]
        DidNotWorking = 2,
    }
}
