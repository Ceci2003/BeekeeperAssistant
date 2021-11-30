namespace BeekeeperAssistant.Web.Controllers
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Web.ViewModels.Feedback;
    using Microsoft.AspNetCore.Mvc;

    public class FeedbackController : Controller
    {
        private readonly IEnumerationMethodsService enumerationMethodsService;

        public FeedbackController(IEnumerationMethodsService enumerationMethodsService)
        {
            this.enumerationMethodsService = enumerationMethodsService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Create(FeedbackType feedbackType)
        {
            if (this.enumerationMethodsService.IsEnumerationDefined(feedbackType))
            {
                this.ViewData["FeedbackType"] = feedbackType;
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateFeedbackInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            // TODO: Add feedback to database and send emails
            return this.Json(inputModel);
        }
    }
}
