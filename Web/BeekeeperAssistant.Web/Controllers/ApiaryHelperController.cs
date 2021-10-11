namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.ApiaryHelpers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ApiaryHelperController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApiaryService apiaryService;
        private readonly IApiaryHelperService apiaryHelperService;

        public ApiaryHelperController(
            UserManager<ApplicationUser> userManager,
            IApiaryService apiaryService,
            IApiaryHelperService apiaryHelperService)
        {
            this.userManager = userManager;
            this.apiaryService = apiaryService;
            this.apiaryHelperService = apiaryHelperService;
        }

        public IActionResult Add(int apiaryId)
        {
            var apiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(apiaryId);
            var viewModel = new AddUserToApiaryInputModel
            {
                ApiaryNumber = apiaryNumber,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int apiaryId, AddUserToApiaryInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            if (currentUser.UserName == inputModel.UserName)
            {
                this.ModelState.AddModelError("UserName", "Не може да добавите себе си!");
                return this.View(inputModel);
            }

            var user = await this.userManager.FindByNameAsync(inputModel.UserName);
            if (this.apiaryHelperService.IsAnApiaryHelper(user.Id, apiaryId))
            {
                this.ModelState.AddModelError("UserName", "Потребителят вече е помошник!");
                return this.View(inputModel);
            }

            await this.apiaryHelperService.Add(user.Id, apiaryId);

            var apiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(apiaryId);

            return this.Redirect($"/Apiary/{apiaryNumber}");
        }
    }
}
