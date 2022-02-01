namespace BeekeeperAssistant.Web.ViewModels.Beehives
{
    using System.Collections.Generic;

    public class SelectBeehivesToAddInTemporaryInputModel
    {
        public int TemporaryId { get; set; }

        public string TemporaryNumber { get; set; }

        public string TemporaryName { get; set; }

        public int SelectedApiaryId { get; set; }

        public IList<SelectBeehiveToAddInTemporaryModel> Beehives { get; set; }
    }
}
