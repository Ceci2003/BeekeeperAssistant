namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.QueenHelpers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class QueenHelperController : Controller
    {
        private readonly IQueenHelperService queenHelperService;
        private readonly UserManager<ApplicationUser> userManager;

        public QueenHelperController(
            IQueenHelperService queenHelperService,
            UserManager<ApplicationUser> userManager)
        {
            this.queenHelperService = queenHelperService;
            this.userManager = userManager;
        }

        public IActionResult All(int id)
        {
            var viewModel = new AllQueenHelpersViewModel
            {
                AllHelpers = this.queenHelperService.GetAllQueenByQueenId<QueenHelperViewModel>(id),
            };

            return this.View(viewModel);
        }

        public IActionResult Edit(string userId, int queenId)
        {
            var inputModel = this.queenHelperService.GetQueenHelper<EditQueenHelperInputModel>(userId, queenId);
            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditQueenHelperInputModel inputModel, string userId, int queenId)
        {
            if (!this.ModelState.IsValid)
            {
                var user = await this.userManager.FindByIdAsync(userId);
                inputModel.UserUserName = user.UserName;
                return this.View(inputModel);
            }

            await this.queenHelperService.Edit(userId, queenId, inputModel.Access);

            return this.Redirect($"/QueenHelper/All/{queenId}");
        }
    }
}
