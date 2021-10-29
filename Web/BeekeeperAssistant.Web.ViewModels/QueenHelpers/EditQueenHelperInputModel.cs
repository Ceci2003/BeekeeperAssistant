namespace BeekeeperAssistant.Web.ViewModels.QueenHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class EditQueenHelperInputModel : IMapFrom<QueenHelper>
    {
        public string UserUserName { get; set; }

        public string UserId { get; set; }

        public int QueenId { get; set; }

        public Access Access { get; set; }

        public int BeehiveId { get; set; }

        public int BeehiveNumber { get; set; }

        public string ApiaryNumber { get; set; }
    }
}
