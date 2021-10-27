namespace BeekeeperAssistant.Web.ViewModels.BeehiveHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class EditBeehiveHelperInputModel : IMapFrom<BeehiveHelper>
    {
        public int BeehiveId { get; set; }

        public string UserId { get; set; }

        public string UserUserName { get; set; }

        public Access Access { get; set; }

    }
}
