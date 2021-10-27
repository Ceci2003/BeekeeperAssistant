﻿namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Services.Messaging;
    using BeekeeperAssistant.Web.ViewModels.ApiaryHelpers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    public class ApiaryHelperController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApiaryService apiaryService;
        private readonly IApiaryHelperService apiaryHelperService;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;

        public ApiaryHelperController(
            UserManager<ApplicationUser> userManager,
            IApiaryService apiaryService,
            IApiaryHelperService apiaryHelperService,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.apiaryService = apiaryService;
            this.apiaryHelperService = apiaryHelperService;
            this.emailSender = emailSender;
            this.configuration = configuration;
        }

        public IActionResult Add(int id)
        {
            var apiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(id);
            var viewModel = new AddUserToApiaryInputModel
            {
                ApiaryNumber = apiaryNumber,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int id, AddUserToApiaryInputModel inputModel)
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
            if (this.apiaryHelperService.IsApiaryHelper(user.Id, id))
            {
                this.ModelState.AddModelError("UserName", "Потребителят вече е помощник!");
                return this.View(inputModel);
            }

            await this.apiaryHelperService.Add(user.Id, id);

            // var apiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(id);
            var helper = await this.userManager.FindByNameAsync(inputModel.UserName);
            await this.emailSender.SendEmailAsync(
                  this.configuration["SendGrid:RecipientEmail"],
                  GlobalConstants.SystemName,
                  currentUser.Email,
                  "Успешно добавен помощник",
                  $"Успешно добавихте <strong>{helper.UserName}</strong> като помощник на пчелин <strong>{inputModel.ApiaryNumber}</strong>.");

            return this.Redirect($"/Apiary/{inputModel.ApiaryNumber}");
        }

        public IActionResult All(int id)
        {
            var viewModel = new AllApiaryHelpersViewModel
            {
                AllHelpers = this.apiaryHelperService.GetAllApiaryHelpersByApiaryId<ApiaryHelperViewModel>(id),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string userId, int apiaryId)
        {
            await this.apiaryHelperService.Delete(userId, apiaryId);
            var apiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(apiaryId);
            return this.Redirect($"/ApiaryHelper/All/{apiaryNumber}");
        }

        public IActionResult Edit(string userId, int apiaryId)
        {
            var inputModel = this.apiaryHelperService.GetApiaryHelper<EditApiaryHelperInputModel>(userId, apiaryId);
            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditApiaryHelperInputModel inputModel, string userId, int apiaryId)
        {
            if (!this.ModelState.IsValid)
            {
                var user = await this.userManager.FindByIdAsync(userId);
                inputModel.UserUserName = user.UserName;
                return this.View(inputModel);
            }

            await this.apiaryHelperService.Edit(userId, apiaryId, inputModel.Access);

            return this.Redirect($"/ApiaryHelper/All/{apiaryId}");
        }
    }
}
