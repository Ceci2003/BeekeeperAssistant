namespace BeekeeperAssistant.Services.Data.Models
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Beehives;

    public class BeehiveDataExportForDFZModel : IMapFrom<Beehive>, IMapFrom<BeehiveDataModel>
    {
        public int Number { get; set; }
    }
}
