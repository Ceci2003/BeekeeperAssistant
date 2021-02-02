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
        private readonly IApiaryService apiaryService;
        private readonly UserManager<ApplicationUser> userManager;

        public ApiaryController(
            IApiaryService apiaryService,
            UserManager<ApplicationUser> userManager)
        {
            this.apiaryService = apiaryService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All()
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
                this.ModelState.AddModelError("Error", "Existing apiary number!");
                return this.View(inputModel);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.apiaryService.Add(inputModel, currenUser.Id);
            return this.Redirect($"/Apiary/{inputModel.Number}");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var currenUser = await this.userManager.GetUserAsync(this.User);
            var inputModel = this.apiaryService.GetById<EditApiaryInputModel>(id, currenUser.Id);
            var numbers = inputModel.Number.Split('-').ToList();
            inputModel.CityCode = numbers[0];
            inputModel.FarmNumber = numbers[1];
            if (inputModel?.CreatorId != currenUser?.Id)
            {
                return this.Forbid();
            }

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditApiaryInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var originalModel = this.apiaryService.GetById<EditApiaryInputModel>(id, currentUser.Id);
            var number = $"{inputModel.CityCode}-{inputModel.FarmNumber}";
            if (this.apiaryService.Exists(number, currentUser.Id) && number != originalModel.Number)
            {
                this.ModelState.AddModelError("Error", "Existing apiary number!");
                return this.View(inputModel);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            inputModel.Number = number;

            await this.apiaryService.EditById(id, inputModel, currentUser.Id);
            return this.Redirect($"/Apiary/{inputModel.Number}");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var apiary = this.apiaryService.GetById<Apiary>(id, currentUser.Id);

            if (!this.apiaryService.Exists(id, currentUser.Id))
            {
                return this.NotFound();
            }

            if (apiary.CreatorId != currentUser.Id)
            {
                return this.Forbid();
            }

            if (!this.ModelState.IsValid)
            {
                return this.Redirect($"/Apiary/{apiary.Number}");
            }

            await this.apiaryService.DeleteById(id, currentUser.Id);
            return this.RedirectToAction(nameof(this.All));
        }
    }
}
