namespace BeekeeperAssistant.Web.ViewModels.Inspection
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class InspectionDataViewModel : IMapFrom<Inspection>
    {
        public int Id { get; set; }

        public int BeehiveId { get; set; }

        public string CreatorId { get; set; }

        [Display(Name = "Дата на прегледа")]
        public DateTime DateOfInspection { get; set; }

        [Display(Name = "Вид на прегледа")]
        public InspectionType InspectionType { get; set; }

        [Display(Name = "Бележка")]
        public string Note { get; set; }

        // Colony info
        [Display(Name = "Роил ли се е")]
        public bool Swarmed { get; set; }

        [Display(Name = "Сила на кошера")]
        public BeehivePower BeehivePower { get; set; }

        [Display(Name = "Нрав на кошера")]
        public BeehiveTemperament BeehiveTemperament { get; set; }

        [Display(Name = "Предприети действия")]
        public BeehiveAction BeehiveAction { get; set; }

        [Display(Name = "Маса на кошера(кг.)")]
        public double Weight { get; set; }

        [Display(Name = "Температура на кошера")]
        public double HiveTemperature { get; set; }

        [Display(Name = "Влажност на кошера")]
        public double HiveHumidity { get; set; }

        // Queen section
        [Display(Name = "Секция майка")]
        public bool IncludeQueenSection { get; set; }

        [Display(Name = "Забелязана майка")]
        public bool QueenSeen { get; set; }

        [Display(Name = "Маточници")]
        public QueenCells QueenCells { get; set; }

        [Display(Name = "Работен статус на майката")]
        public QueenWorkingStatus QueenWorkingStatus { get; set; }

        [Display(Name = "Сила на майката")]
        public QueenPowerStatus QueenPowerStatus { get; set; }

        // Brood
        [Display(Name = "Секция пило")]
        public bool IncludeBrood { get; set; }

        [Display(Name = "Яйца")]
        public bool Eggs { get; set; }

        [Display(Name = "Запечатано пило")]
        public bool ClappedBrood { get; set; }

        [Display(Name = "Излюпващо се пило")]
        public bool UnclappedBrood { get; set; }

        // Frames with
        [Display(Name = "Секция пити с")]
        public bool IncludeFramesWith { get; set; }

        [Display(Name = "Пити с пчели")]
        public int FramesWithBees { get; set; }

        [Display(Name = "Пити с пило")]
        public int FramesWithBrood { get; set; }

        [Display(Name = "Пити с мед")]
        public int FramesWithHoney { get; set; }

        [Display(Name = "Пити с прашец")]
        public int FramesWithPollen { get; set; }

        // Activity
        [Display(Name = "Секция активност")]
        public bool IncludeActivity { get; set; }

        [Display(Name = "Активност на пчелите")]
        public Activity BeeActivity { get; set; }

        [Display(Name = "Oриентационни полети")]
        public Activity OrientationActivity { get; set; }

        [Display(Name = "Принос на прашец")]
        public Activity PollenActivity { get; set; }

        [Display(Name = "Принос на мед")]
        public Activity ForragingActivity { get; set; }

        [Display(Name = "Пчели в минута")]
        public int BeesPerMinute { get; set; }

        // Storages
        [Display(Name = "Секция запаси")]
        public bool IncludeStorage { get; set; }

        [Display(Name = "Запаси от мед")]
        public StoragePower StoredHoney { get; set; }

        [Display(Name = "Запаси от прашец")]
        public StoragePower StoredPollen { get; set; }

        // Spotted problems
        [Display(Name = "Секция забелязани проблеми")]
        public bool IncludeSpottedProblem { get; set; }

        [Display(Name = "Заболяване")]
        public string Disease { get; set; }

        [Display(Name = "Третиране с")]
        public string Treatment { get; set; }

        [Display(Name = "Вредители")]
        public string Pests { get; set; }

        [Display(Name = "Хищници")]
        public string Predators { get; set; }

        // Weather info
        [Display(Name = "СекцияМетеорологичниУсловия")]
        public bool IncludeWeatherInfo { get; set; }

        [Display(Name = "Условия")]
        public string Conditions { get; set; }

        [Display(Name = "Температура")]
        public double WeatherTemperature { get; set; }

        [Display(Name = "Влажност")]
        public double WeatherHumidity { get; set; }
    }
}
