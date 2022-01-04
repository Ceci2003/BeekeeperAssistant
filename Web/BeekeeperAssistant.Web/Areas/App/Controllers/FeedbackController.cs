namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Feedbacks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class FeedbackController : AppBaseController
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
            return View();
        }

        public IActionResult Create(FeedbackType feedbackType)
        {
            if (enumerationMethodsService.IsEnumerationDefined(feedbackType))
            {
                ViewData["FeedbackType"] = feedbackType;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFeedbackInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            var currentUser = await userManager.GetUserAsync(User);

            await feedbackService.CreateAsync(currentUser.Id, inputModel.Title, inputModel.Body, inputModel.FeedbackType);

            TempData[GlobalConstants.SuccessMessage] = "Успешно изпратено запитване!";
            return RedirectToAction("Index", "Home");
        }
    }
}
