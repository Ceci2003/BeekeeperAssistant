﻿namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.BeehiveHelpers;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class BeehiveHelperController : AppBaseController
    {
        private readonly IBeehiveHelperService beehiveHelperService;
        private readonly IBeehiveService beehiveService;
        private readonly UserManager<ApplicationUser> userManager;

        public BeehiveHelperController(
            IBeehiveHelperService beehiveHelperService,
            IBeehiveService beehiveService,
            UserManager<ApplicationUser> userManager)
        {
            this.beehiveHelperService = beehiveHelperService;
            this.beehiveService = beehiveService;
            this.userManager = userManager;
        }

        // DONE []
        public IActionResult All(int id)
        {
            var beehive = this.beehiveService.GetBeehiveById<ByIdBeehiveViewModel>(id);

            var viewModel = new AllBeehiveHelperViewModel
            {
                AllHelpers = this.beehiveHelperService.GetAllBeehiveHelpersByBeehiveId<BeehiveHelperViewModel>(id),
                BeehiveId = id,
                BeehiveNumber = beehive.Number,
                ApiaryNumber = beehive.ApiaryNumber,
            };

            return this.View(viewModel);
        }

        public IActionResult Edit(string userId, int beehiveId)
        {
            var inputModel = this.beehiveHelperService.GetBeehiveHelper<EditBeehiveHelperInputModel>(userId, beehiveId);
            inputModel.BeehiveId = beehiveId;
            inputModel.BeehiveNumber = this.beehiveService.GetBeehiveById<ByIdBeehiveViewModel>(beehiveId).Number;
            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBeehiveHelperInputModel inputModel, string userId, int beehiveId)
        {
            if (!this.ModelState.IsValid)
            {
                var user = await this.userManager.FindByIdAsync(userId);
                inputModel.UserUserName = user.UserName;
                inputModel.BeehiveId = beehiveId;
                inputModel.BeehiveNumber = this.beehiveService.GetBeehiveById<ByIdBeehiveViewModel>(beehiveId).Number;
                return this.View(inputModel);
            }

            await this.beehiveHelperService.EditAsync(userId, beehiveId, inputModel.Access);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно редактиран помощник!";
            return this.RedirectToAction(nameof(this.All), new { id = beehiveId });
        }
    }
}
