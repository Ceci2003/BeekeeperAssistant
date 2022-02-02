namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SelectApiaryToMoveBeehiveIn
    {
        public int BeehiveId { get; set; }

        public int BeehiveNumber { get; set; }

        public int BeehiveApiaryId { get; set; }

        public string BeehiveApiaryNumber { get; set; }

        public string BeehiveApiaryName { get; set; }

        [Display(Name = "Пчелин")]
        public int SelectedApiaryId { get; set; }

        public IEnumerable<KeyValuePair<int, string>> AllApiaries { get; set; }
    }
}
