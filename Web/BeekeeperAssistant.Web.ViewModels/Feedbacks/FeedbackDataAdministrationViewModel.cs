namespace BeekeeperAssistant.Web.ViewModels.Feedbacks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class FeedbackDataAdministrationViewModel : IMapFrom<Feedback>
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public string UserUsername { get; set; }
    }
}
