namespace BeekeeperAssistant.Web.Controllers
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Authorize]
    public class ApiaryController : BaseController
    {
        private readonly IApiaryService apiaryService;
        private readonly UserManager<ApplicationUser> userManager;

        public ApiaryController(
            IApiaryService apiaryService,
            UserManager<ApplicationUser> userManager)
        {
            this.apiaryService = apiaryService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All ()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var allApiarires = this.apiaryService.GetAllByUser<UserApiaryViewModel>(currentUser.Id);
            var viewModel = new AllUserApiariesViewModel()
            {
                AllUserApiaries = allApiarires,
            };
            return this.View(viewModel);
        }

        public async Task<IActionResult> ByNumber(string number)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var apiary = this.apiaryService.GetByNUmber<ApiaryDataViewModel>(number, currentUser.Id);
            if (apiary == null)
            {
                return this.Forbid();
            }

            return this.View(apiary);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateApiaryInputModel inputModel)
        {
            var currenUser = await this.userManager.GetUserAsync(this.User);
            if (this.apiaryService.Exists(inputModel.Number, currenUser.Id))
            {
                this.ModelState.AddModelError("Number", "Existing apiary number!");
                return this.View(inputModel);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.apiaryService.Add(inputModel, currenUser.Id);
            return this.Redirect($"/Apiary/{inputModel.Number}");
        }
    }
}
