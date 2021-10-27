namespace BeekeeperAssistant.Web.ViewModels.ApiaryHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class EditApiaryHelperInputModel : IMapFrom<ApiaryHelper>
    {
        public int ApiaryId { get; set; }

        public string UserId { get; set; }

        public string UserUserName { get; set; }

        public Access Access { get; set; }
    }
}
