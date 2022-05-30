using BeekeeperAssistant.Data.Filters.Models;
using System.Collections.Generic;

namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    public class AllApiaryUserApiariesFiltersViewModel
    {
        public IList<string> ModelProperties { get; set; }

        public IList<string> ModelPropertiesDisplayNames { get; set; }
    }
}