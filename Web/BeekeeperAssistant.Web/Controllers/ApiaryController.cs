﻿namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [Authorize]
    public class ApiaryController : BaseController
    {
        private readonly IApiaryService apiaryService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBeehiveService beehiveService;

        public ApiaryController(
            IApiaryService apiaryService,
            UserManager<ApplicationUser> userManager,
            IBeehiveService beehiveService)
        {
            this.apiaryService = apiaryService;
            this.userManager = userManager;
            this.beehiveService = beehiveService;
        }

        public async Task<IActionResult> GetByNumber(string apiNumber)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var apiary = this.apiaryService.GetUserApiaryByNumber(apiNumber, currentUser);

            if (apiary == null)
            {
                return this.Forbid();
            }

            var allApiaryBeehives = this.beehiveService.GetAllUserBeehivesByApiaryId<UserBeehiveViewModel>(apiary.Id);

            var viewModel = new AllUserBeehivesViewModel()
            {
                AllUserBeehives = allApiaryBeehives,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            await this.apiaryService.DeleteById(id, currentUser);
            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateApiaryInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            if (this.apiaryService.UserApiaryExists(inputModel.Number, currentUser))
            {
                this.ModelState.AddModelError("Number", "Invalid apiary number!");
                return this.View(inputModel);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.apiaryService.AddUserApiary(currentUser, inputModel);

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var apiary = this.apiaryService.GetUserApiaryById<EditApiaryInputModel>(id, currentUser);
            return this.View(apiary);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditApiaryInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (this.apiaryService.UserApiaryExists(inputModel.Number, currentUser))
            {
                this.ModelState.AddModelError("Number", "Invalid apiary number!");
                return this.View(inputModel);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.apiaryService.EditUserApiaryById(id, currentUser, inputModel);
            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> All()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var allApiarires = this.apiaryService.GetAllUserApiaries<UserApiaryViewModel>(currentUser.Id);
            var viewModel = new AllUserApiariesViewModel()
            {
                AllUserApiaries = allApiarires,
            };
            return this.View(viewModel);
        }
    }
}
