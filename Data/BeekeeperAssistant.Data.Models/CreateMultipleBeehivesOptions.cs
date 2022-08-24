namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum CreateMultipleBeehivesOptions
    {
        [Display(Name = "Добави само номерата, които не съществуват")]
        AddOnlyBeehivesThatDontExist = 1,
        [Display(Name = "Замени съществуващите номера")]
        ReplaceExistingBeehives = 2,
    }
}
