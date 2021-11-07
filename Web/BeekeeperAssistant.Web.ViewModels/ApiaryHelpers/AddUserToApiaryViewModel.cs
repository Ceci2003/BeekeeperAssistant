using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Web.ViewModels.ApiaryHelpers
{
    public class AddUserToApiaryViewModel
    {
        public int ApiaryId { get; set; }

        public string ApiaryNumber { get; set; }

        public AddUserToApiaryInputModel InputModel { get; set; }
    }
}
