namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum Access
    {
        [Display(Name = "Четене")]
        Read = 0,

        [Display(Name = "Четене и писане")]
        ReadWrite = 2,
    }
}
