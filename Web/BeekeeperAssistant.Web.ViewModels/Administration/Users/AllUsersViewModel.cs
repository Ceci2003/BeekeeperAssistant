using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Web.ViewModels.Administration.Users
{
    public class AllUsersViewModel
    {
        public IEnumerable<UserViewModel> AllUsers { get; set; }
    }
}
