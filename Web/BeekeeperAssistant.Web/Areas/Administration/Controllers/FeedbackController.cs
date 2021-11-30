namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    using BeekeeperAssistant.Web.ViewModels.Feedbacks;
    using Microsoft.AspNetCore.Mvc;

    public class FeedbackController : AdministrationController
    {
        public IActionResult All(int feedbacksPage = 1, int helpsPage)
        {
            var viewModel = new AllFeedabacksAdministrationViewModel();
            return this.View();
        }

    }
}
