using BeekeeperAssistant.Data.Models;
using BeekeeperAssistant.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Web.ViewModels.Administration.Apiaries
{
    public class AdministrationAllApiaryAllApiariesViewModel : IMapFrom<Apiary>
    {
        public string Number { get; set; }

        public bool IsDeleted { get; set; }

        public string CreatorUserName { get; set; }

        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
