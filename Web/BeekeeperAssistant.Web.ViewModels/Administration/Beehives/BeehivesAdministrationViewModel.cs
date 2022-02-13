namespace BeekeeperAssistant.Web.ViewModels.Administration.Beehives
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class BeehivesAdministrationViewModel : IMapFrom<Beehive>
    {
        public int Id { get; set; }

        public string ApiaryId{ get; set; }

        public string ApiaryNumber { get; set; }

        public string ApiaryName{ get; set; }

        public int Number { get; set; }

        public bool IsDeleted { get; set; }
    }
}
