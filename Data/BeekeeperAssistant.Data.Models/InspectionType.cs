namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum InspectionType
    {
        [Display(Name = "Първи пролетен")]
        FirstSpring = 1,
        [Display(Name = "Първи зимен")]
        FirstWinter = 2,
        [Display(Name = "Периодичен")]
        Periodical = 3,
    }
}
