namespace BeekeeperAssistant.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum BeehiveAction
    {
        [Display(Name = "Придадена майка")]
        AddedQueen = 1,
        [Display(Name = "Сменена майка")]
        Requeened = 2,
        [Display(Name = "Разместени пити с пило")]
        SwapedBroodFrames = 3,
        [Display(Name = "Добавен мишепредпазител")]
        AddedEntranceReducer = 4,
        [Display(Name = "Премахнат мишепредпазител")]
        RemovedEntranceReducer = 5,
        [Display(Name = "Добавен магазин/корпус с пило")]
        AddedBroodSuper = 6,
        [Display(Name = "Премахнат магазин/корпус с пило")]
        RemovedBroodSuper = 7,
        [Display(Name = "Обединен със семейство")]
        CombinedWithStrongHive = 8,
        [Display(Name = "Добавен магазин/корпус с мед")]
        AddedHoneySuper = 9,
        [Display(Name = "Премахнат магазин/корпус с мед")]
        RemovedHoneySuper = 10,
        [Display(Name = "Инсталиран капан за кошерен бръмбар")]
        InstalledHiveBeetleTrap = 11,
        [Display(Name = "Добавена ханеманова решетка")]
        AddedQueenExcluder = 12,
        [Display(Name = "Премахната ханеманова решетка")]
        RemovedQueenExcluder = 13,
        [Display(Name = "Подменено оборудване")]
        ReplacedEquipment = 14,
        [Display(Name = "Преброени акари")]
        MiteCount = 15,
        [Display(Name = "Раделяне на кошера")]
        SplitHive = 16,
        [Display(Name = "Почистване на кошера")]
        CleanedHive = 17,
        [Display(Name = "Добавена табла")]
        AddedInnerCover = 18,
        [Display(Name = "Премахната табла")]
        RemovedInnerCover = 19,
        [Display(Name = "Добавена хранилка")]
        AddedFeeder = 20,
        [Display(Name = "Премахната хранилка")]
        RemovedFeeder = 21,
        [Display(Name = "Добавен прашецоуловител")]
        AddedPollenTrap = 22,
        [Display(Name = "Премахнат прашецоуловител")]
        RemovedPollenTrap = 23,
        [Display(Name = "Нахранен")]
        FedHive = 24,
        [Display(Name = "Добавени пити")]
        AddedFrames = 25,
        [Display(Name = "Премахнати пити")]
        RemovedFrames = 26,
        [Display(Name = "Няма")]
        None = 27,
    }
}
