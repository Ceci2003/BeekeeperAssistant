namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using BeekeeperAssistant.Web.ViewModels.QueenHelpers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class QueenHelperController : BaseController
    {
        private readonly IQueenHelperService queenHelperService;
        private readonly IBeehiveService beehiveService;
        private readonly UserManager<ApplicationUser> userManager;

        public QueenHelperController(
            IQueenHelperService queenHelperService,
            IBeehiveService beehiveService,
            UserManager<ApplicationUser> userManager)
        {
            this.queenHelperService = queenHelperService;
            this.beehiveService = beehiveService;
            this.userManager = userManager;
        }

        public IActionResult All(int id)
        {
            var beehive = this.beehiveService.GetBeehiveByQueenId<BeehiveViewModel>(id);

            var viewModel = new AllQueenHelperViewModel
            {
                AllHelpers = this.queenHelperService.GetAllQueenByQueenId<QueenHelperViewModel>(id),
                BeehiveId = beehive.Id,
                BeehiveNumber = beehive.Number,
                ApiaryNumber = beehive.ApiaryNumber,
                QueenId = id,
            };

            return this.View(viewModel);
        }

        public IActionResult Edit(string userId, int queenId)
        {
            var inputModel = this.queenHelperService.GetQueenHelper<EditQueenHelperInputModel>(userId, queenId);
            inputModel.QueenId = queenId;

            var beehive = this.beehiveService.GetBeehiveByQueenId<BeehiveViewModel>(queenId);
            inputModel.BeehiveId = beehive.Id;
            inputModel.BeehiveNumber = beehive.Number;
            inputModel.ApiaryNumber = beehive.ApiaryNumber;

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditQueenHelperInputModel inputModel, string userId, int queenId)
        {
            if (!this.ModelState.IsValid)
            {
                var user = await this.userManager.FindByIdAsync(userId);
                inputModel.UserUserName = user.UserName;
                inputModel.QueenId = queenId;

                var beehive = this.beehiveService.GetBeehiveByQueenId<BeehiveViewModel>(queenId);
                inputModel.BeehiveId = beehive.Id;
                inputModel.BeehiveNumber = beehive.Number;
                inputModel.ApiaryNumber = beehive.ApiaryNumber;

                return this.View(inputModel);
            }

            await this.queenHelperService.EditAsync(userId, queenId, inputModel.Access);

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно редактиран помощник!";
            return this.Redirect($"/QueenHelper/All/{queenId}");
        }
    }
}
