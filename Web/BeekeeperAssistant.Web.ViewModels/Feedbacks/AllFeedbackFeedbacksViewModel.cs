namespace BeekeeperAssistant.Web.ViewModels.Feedbacks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AllFeedbackFeedbacksViewModel
    {
        public ICollection<FeedbackDataAdministrationViewModel> AllFeedbackFeedbacks { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
