namespace BeekeeperAssistant.Web.Controllers
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Authorize]
    public class BeehiveController : BaseController
    {
        private readonly IApiaryService apiaryService;
        private readonly UserManager<ApplicationUser> userManager;

        public BeehiveController(IApiaryService apiaryService, UserManager<ApplicationUser> userManager)
        {
            this.apiaryService = apiaryService;
            this.userManager = userManager;
        }

        public IActionResult ById(int beehiveId)
        {
            return this.View();
        }

        public async Task<IActionResult> Create(int? id)
        {
            var inputModel = new CreateBeehiveInputModel();

            if (id == null)
            {
                var currentUser = await this.userManager.GetUserAsync(this.User);
                inputModel.AllApiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
            }

            return this.View(inputModel);
        }

        [HttpPost]
        public IActionResult Create(int? id, CreateBeehiveInputModel inputModel)
        {
            var apiId = id == null ? inputModel.ApiaryId : id;

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            // TODO: Add validation attributes
            // TODO: Add servies for beehive service
            // TODO: Create crud for beehives
            // TODO: Start implementing queens
            return this.RedirectToAction(nameof(this.ById));
        }

        public IActionResult Edit(int id)
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Edit(EditBeehiveInputModel inputModel)
        {
            return this.View();
        }
    }
}
