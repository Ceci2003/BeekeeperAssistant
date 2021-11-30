namespace BeekeeperAssistant.Web.Controllers
{
    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Feedbacks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class FeedbackController : Controller
    {
        private readonly IEnumerationMethodsService enumerationMethodsService;
        private readonly IFeedbackService feedbackService;
        private readonly UserManager<ApplicationUser> userManager;

        public FeedbackController(
            IEnumerationMethodsService enumerationMethodsService,
            IFeedbackService feedbackService,
            UserManager<ApplicationUser> userManager)
        {
            this.enumerationMethodsService = enumerationMethodsService;
            this.feedbackService = feedbackService;
            this.userManager = userManager;
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
        public async Task<IActionResult> Create(CreateFeedbackInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            await this.feedbackService.CreateAsync(currentUser.Id, inputModel.Title, inputModel.Body, inputModel.FeedbackType);

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно изпратено запитване!";
            return this.Redirect("/");
        }
    }
}
