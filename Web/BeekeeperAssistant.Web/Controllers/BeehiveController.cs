﻿namespace BeekeeperAssistant.Web.Controllers
{
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class BeehiveController : BaseController
    {
        private readonly IApiaryService apiaryService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBeehiveService beehiveService;

        public BeehiveController(
            IApiaryService apiaryService,
            UserManager<ApplicationUser> userManager,
            IBeehiveService beehiveService)
        {
            this.apiaryService = apiaryService;
            this.userManager = userManager;
            this.beehiveService = beehiveService;
        }

        public async Task<IActionResult> All(int page = 1)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var beehives = this.beehiveService.GetAllUserBeehives<BeehiveViewModel>(currentUser.Id);

            var viewModel = new AllBeehivesViewModel
            {
                AllBeehives = beehives,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> ById(int beehiveId)
        {
            var viewModel = this.beehiveService.GetBeehiveById<BeehiveDataViewModel>(beehiveId);
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            if (viewModel.CreatorId != currentUser.Id)
            {
                return this.BadRequest();
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Create(int? id)
        {
            var inputModel = new CreateBeehiveInputModel();

            if (id == null)
            {
                var currentUser = await this.userManager.GetUserAsync(this.User);
                inputModel.AllApiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
            }
            else
            {
                inputModel.ApiaryId = id.Value;
            }

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBeehiveInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            var beehiveId = await this.beehiveService
                .CreateUserBeehiveAsync(
                currentUser.Id,
                inputModel.Number,
                inputModel.BeehiveSystem,
                inputModel.BeehiveType,
                inputModel.Date,
                inputModel.ApiaryId,
                inputModel.BeehivePower,
                inputModel.HasDevice,
                inputModel.HasPolenCatcher,
                inputModel.HasPropolisCatcher);

            var apiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(beehiveId);

            return this.Redirect($"/Beehive/{apiaryNumber}/{beehiveId}");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var inputModel = this.beehiveService.GetBeehiveById<EditBeehiveInputModel>(id);
            inputModel.AllApiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditBeehiveInputModel inputModel)
        {
            var beehiveId = await this.beehiveService.EditUserBeehiveAsync(
                id,
                inputModel.Number,
                inputModel.BeehiveSystem,
                inputModel.BeehiveType,
                inputModel.Date,
                inputModel.ApiaryId,
                inputModel.BeehivePower,
                inputModel.HasDevice,
                inputModel.HasPolenCatcher,
                inputModel.HasPropolisCatcher);

            var apiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(beehiveId);

            return this.Redirect($"/Beehive/{apiaryNumber}/{beehiveId}");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataViewModel>(id);
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (beehive.CreatorId != currentUser.Id)
            {
                return this.BadRequest();
            }

            var apiaryNumber = await this.beehiveService.DeleteBeehiveByIdAsync(id);
            return this.Redirect($"/Apiary/{apiaryNumber}");
        }
    }
}
