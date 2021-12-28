namespace BeekeeperAssistant.Web.ViewModels.Feedbacks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class AdministrationByIdFeedbackViewModel : IMapFrom<Feedback>
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime CreatedOn { get; set; }

        public FeedbackType FeedbackType { get; set; }

        public string UserUserName { get; set; }
    }
}
