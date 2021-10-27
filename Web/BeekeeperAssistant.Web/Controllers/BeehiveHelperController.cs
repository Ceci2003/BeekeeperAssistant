namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.BeehiveHelpers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class BeehiveHelperController : Controller
    {
        private readonly IBeehiveHelperService beehiveHelperService;
        private readonly UserManager<ApplicationUser> userManager;

        public BeehiveHelperController(
            IBeehiveHelperService beehiveHelperService,
            UserManager<ApplicationUser> userManager)
        {
            this.beehiveHelperService = beehiveHelperService;
            this.userManager = userManager;
        }

        public IActionResult All(int id)
        {
            var viewModel = new AllBeehiveHelpersViewModel
            {
                AllHelpers = this.beehiveHelperService.GetAllBeehiveHelpersByBeehiveId<BeehiveHelperViewModel>(id),
            };

            return this.View(viewModel);
        }

        public IActionResult Edit(string userId, int beehiveId)
        {
            var inputModel = this.beehiveHelperService.GetBeehiveHelper<EditBeehiveHelperInputModel>(userId, beehiveId);
            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBeehiveHelperInputModel inputModel, string userId, int beehiveId)
        {
            if (!this.ModelState.IsValid)
            {
                var user = await this.userManager.FindByIdAsync(userId);
                inputModel.UserUserName = user.UserName;
                return this.View(inputModel);
            }

            await this.beehiveHelperService.Edit(userId, beehiveId, inputModel.Access);

            return this.Redirect($"/BeehiveHelper/All/{beehiveId}");
        }
    }
}
