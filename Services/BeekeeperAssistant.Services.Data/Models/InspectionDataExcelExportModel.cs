namespace BeekeeperAssistant.Services.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class InspectionDataExcelExportModel : IMapFrom<Inspection>
    {
        public InspectionType InspectionType { get; set; }

        public string Note { get; set; }

        // Colony info
        public bool Swarmed { get; set; }

        public BeehivePower BeehivePower { get; set; }

        public double Weight { get; set; }

        public double HiveTemperature { get; set; }

        public double HiveHumidity { get; set; }

        // Queen section
        public bool IncludeQueenSection { get; set; }

        public bool QueenSeen { get; set; }

        public QueenCells QueenCells { get; set; }

        public QueenWorkingStatus QueenWorkingStatus { get; set; }

        public QueenPowerStatus QueenPowerStatus { get; set; }

        // Brood
        public bool IncludeBrood { get; set; }

        public bool Eggs { get; set; }

        public bool ClappedBrood { get; set; }

        public bool UnclappedBrood { get; set; }

        // Frames with
        public bool IncludeFramesWith { get; set; }

        public int FramesWithBees { get; set; }

        public int FramesWithBrood { get; set; }

        public int FramesWithHoney { get; set; }

        public int FramesWithPollen { get; set; }

        // Activity
        public bool IncludeActivity { get; set; }

        public Activity BeeActivity { get; set; }

        public Activity OrientationActivity { get; set; }

        public Activity PollenActivity { get; set; }

        public Activity ForragingActivity { get; set; }

        public int BeesPerMinute { get; set; }

        // Storages
        public bool IncludeStorage { get; set; }

        public StoragePower StoredHoney { get; set; }

        public StoragePower StoredPollen { get; set; }

        // Spotted problems
        public bool IncludeSpottedProblem { get; set; }

        public string Disease { get; set; }

        public string Treatment { get; set; }

        public string Pests { get; set; }

        public string Predators { get; set; }

        // Weather info
        public bool IncludeWeatherInfo { get; set; }

        public string Conditions { get; set; }

        public double WeatherTemperature { get; set; }

        public double WeatherHumidity { get; set; }
    }
}
