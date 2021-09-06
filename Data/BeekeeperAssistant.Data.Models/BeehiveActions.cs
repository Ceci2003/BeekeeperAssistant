namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum BeehiveActions
    {
        [Display(Name = "")]
        AddedQueen = 1,
        [Display(Name = "")]
        Requeened = 2,
        [Display(Name = "")]
        SwapedBroodFrames = 3,
        [Display(Name = "")]
        AddedEntranceReducer = 4,
        [Display(Name = "")]
        RemovedEntranceReducer = 5,
        [Display(Name = "Добавен магазин/корпус с пило")]
        AddedBroodSuper = 6,
        [Display(Name = "")]
        RemovedBroodSuper = 7,
        [Display(Name = "Обединено със семейство")]
        CombinedWithStrongHive = 8,
        [Display(Name = "")]
        AddedHoneySuper = 9,
        [Display(Name = "")]
        RemovedHoneySuper = 10,
        [Display(Name = "")]
        InstalledHiveBeetleTrap = 11,
        [Display(Name = "")]
        AddedQueenExcluder = 12,
        [Display(Name = "")]
        RemovedQueenExcluder = 13,
        [Display(Name = "")]
        ReplacedEquipment = 14,
        [Display(Name = "")]
        MiteCount = 15,
        [Display(Name = "")]
        SplitHive = 16,
        [Display(Name = "")]
        CleanedHive = 17,
        [Display(Name = "")]
        AddedInnerCover = 18,
        [Display(Name = "")]
        RemovedInnerCover = 19,
        [Display(Name = "")]
        AddedFeeder = 20,
        [Display(Name = "")]
        RemovedFeeder = 21,
        [Display(Name = "")]
        AddedPollenTrap = 22,
        [Display(Name = "")]
        RemovedPollenTrap = 23,
        [Display(Name = "")]
        FedHive = 24,
        [Display(Name = "")]
        AddedFrames = 25,
        [Display(Name = "")]
        RemovedFrames = 26,
    }
}
