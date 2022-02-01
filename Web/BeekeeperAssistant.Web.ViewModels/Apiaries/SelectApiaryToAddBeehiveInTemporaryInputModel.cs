namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SelectApiaryToAddBeehiveInTemporaryInputModel
    {
        public int TemporaryId { get; set; }

        public string TemporaryNumber { get; set; }

        public string TemporaryName { get; set; }

        [Display(Name = "Пчелин")]
        public int SelectedApiaryId { get; set; }

        public IEnumerable<KeyValuePair<int, string>> AllApiaries { get; set; }
    }
}
