namespace BeekeeperAssistant.Data.Models
{
    using BeekeeperAssistant.Data.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Inspection : BaseDeletableModel<int>
    {
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public int BeehiveId { get; set; }

        public virtual Beehive Beehive { get; set; }

        public DateTime DateOfInspection { get; set; }

        public InspectionType InspectionType { get; set; }

        public string Note { get; set; }

        // Colony info
        public bool Swarmed { get; set; }

        public BeehivePower BeehivePower { get; set; }

        public BeehiveTemperament BeehiveTemperament { get; set; }

        public BeehiveActions BeehiveActions { get; set; }

        public int Weight { get; set; }

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

        public int Bees { get; set; }

        public int Brood { get; set; }

        public int Honey { get; set; }

        public int Pollen { get; set; }

        // Activity
        public bool IncludeActivity { get; set; }

        public Activity BeeActivity { get; set; }

        public Activity OrientationActivity { get; set; }

        public Activity PolenActivity { get; set; }

        public Activity ForragingActivity { get; set; }

        public int BeesPerMinute { get; set; }

        // Storages
        public StoragePower StoredHoney { get; set; }

        public StoragePower StoredPollen { get; set; }

        // Spotted problems
        public bool IncludeSpottedProblem { get; set; }

        public string Disease { get; set; }

        public string Treatment { get; set; }

        public string Pests { get; set; }

        public string Predation { get; set; }

        // Weather info
        public bool IncludeWeatherInfo { get; set; }

        public string Conditions { get; set; }

        public double WeatherTemperature { get; set; }

        public double WeatherHumidity { get; set; }
    }
}
