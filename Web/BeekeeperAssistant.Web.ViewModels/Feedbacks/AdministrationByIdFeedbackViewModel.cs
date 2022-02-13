namespace BeekeeperAssistant.Web.ViewModels.Feedbacks
{
    using System;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;
    using BeekeeperAssistant.Web.ViewModels.Administration.Feedbacks;

    public class AdministrationByIdFeedbackViewModel : IMapFrom<Feedback>
    {
        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime CreatedOn { get; set; }

        public FeedbackType FeedbackType { get; set; }

        public string UserUserName { get; set; }

        public string UserEmail { get; set; }
    }
}
