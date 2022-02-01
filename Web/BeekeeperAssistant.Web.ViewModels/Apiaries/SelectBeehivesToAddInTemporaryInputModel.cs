namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SelectBeehivesToAddInTemporaryInputModel
    {
        public int TemporaryId { get; set; }

        public string TemporaryNumber { get; set; }

        public string TemporaryName { get; set; }

        public IEnumerable<KeyValuePair<int, string>> AllBeehives { get; set; }
    }
}
