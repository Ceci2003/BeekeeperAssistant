namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class SelectBeehiveToAddInTemporaryModel : IMapFrom<Beehive>, IMapFrom<BeehiveDataModel>
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public bool IsChecked { get; set; }
    }
}
