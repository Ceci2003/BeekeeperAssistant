namespace BeekeeperAssistant.Web.ViewModels.Harvest
{
    using System;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class HarvestDatavVewModel : IMapFrom<Harvest>
    {
        public int BeehiveId { get; set; }

        public virtual Beehive Beehive { get; set; }

        public string HarvestName { get; set; }

        public DateTime DateOfHarves { get; set; }

        public string Product { get; set; }

        public HoneyType HoneyType { get; set; }

        public string Note { get; set; }

        public int Amount { get; set; }
    }
}
