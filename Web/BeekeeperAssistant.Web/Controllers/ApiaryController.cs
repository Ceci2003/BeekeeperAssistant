namespace BeekeeperAssistant.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ApiaryController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApiaryService apiaryService;

        public ApiaryController(
            UserManager<ApplicationUser> userManager,
            IApiaryService apiaryService)
        {
            this.userManager = userManager;
            this.apiaryService = apiaryService;
        }

        public async Task<IActionResult> All()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var viewModel = new AllApiariesViewModel
            {
                AllUserApiaries = this.apiaryService.GetAllUserApiaries<ApiaryViewModel>(currentUser.Id),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> ByNumber(string apiaryNumber)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var viewModel = this.apiaryService.GetUserApiaryByNumber<ApiaryDataViewModel>(currentUser.Id, apiaryNumber);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateApiaryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            // TODO: Add VALIDATION ATTRIBUTE if the apiary exists in the database!
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var apiaryNumber = await this.apiaryService.CreateUserApiaryAsync(currentUser.Id, inputModel.Number, inputModel.Name, inputModel.ApiaryType, inputModel.Adress);

            return this.RedirectToAction(nameof(this.ByNumber), new { apiaryNumber });
        }

        public IActionResult Edit(int id)
        {
            var viewModel = this.apiaryService.GetApiaryById<EditApiaryInputModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditApiaryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var apiaryNumber = await this.apiaryService.EditApiaryByIdAsync(id, inputModel.Number, inputModel.Name, inputModel.ApiaryType, inputModel.Adress);

            return this.RedirectToAction(nameof(this.ByNumber), new { apiaryNumber });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await this.apiaryService.DeleteApiaryByIdAsync(id);
            return this.RedirectToAction(nameof(this.All));
        }
    }
}
