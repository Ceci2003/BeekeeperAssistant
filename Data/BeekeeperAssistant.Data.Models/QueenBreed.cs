namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum QueenBreed
    {
        [Display(Name = "Европейска тъмна пчела")]
        EuropeanDarkBee = 0,
        [Display(Name = "Карниолска медоносна пчела")]
        CarniolanHoneybee = 1,
        [Display(Name = "Кавказка медоносна пчела")]
        CaucasianHoneybee = 2,
        [Display(Name = "Kръстосан вид")]
        CrossbredSpecies = 3,
        [Display(Name = "Друга")]
        Other = 4,
    }
}
