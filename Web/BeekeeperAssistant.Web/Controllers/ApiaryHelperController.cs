namespace BeekeeperAssistant.Web.Controllers
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

    public class ApiaryHelperController : BaseController
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

        // DONE []
        public IActionResult Add(int id)
        {
            var apiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(id);
            var viewModel = new AddUserToApiaryInputModel
            {
                ApiaryNumber = apiaryNumber,
                ApiaryId = id,
            };
            return this.View(viewModel);
        }

        // DONE []
        [HttpPost]
        public async Task<IActionResult> Add(int id, AddUserToApiaryInputModel viewModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            viewModel.ApiaryId = id;
            viewModel.ApiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(id);

            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            if (currentUser.UserName == viewModel.UserName)
            {
                this.ModelState.AddModelError("UserName", "Не може да добавите себе си!");
                return this.View(viewModel);
            }

            var user = await this.userManager.FindByNameAsync(viewModel.UserName);
            if (this.apiaryHelperService.IsApiaryHelper(user.Id, id))
            {
                this.ModelState.AddModelError("UserName", "Потребителят вече е помощник!");
                return this.View(viewModel);
            }

            await this.apiaryHelperService.AddAsync(user.Id, id);

            var helper = await this.userManager.FindByNameAsync(viewModel.UserName);
            await this.emailSender.SendEmailAsync(
                  this.configuration["SendGrid:RecipientEmail"],
                  GlobalConstants.SystemName,
                  currentUser.Email,
                  "Успешно добавен помощник",
                  $"Успешно добавихте <strong>{helper.UserName}</strong>, като помощник на пчелин: <strong>{viewModel.ApiaryNumber}</strong>.");

            await this.emailSender.SendEmailAsync(
                  this.configuration["SendGrid:RecipientEmail"],
                  GlobalConstants.SystemName,
                  helper.Email,
                  "Успешно добавен помощник",
                  $"Успешно бяхте добавени, като помощник на пчелин: <strong>{viewModel.ApiaryNumber}</strong> от <strong>{currentUser.Email}</strong>.");

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно добавен помощник!";
            return this.Redirect($"/ApiaryHelper/All/{id}");
        }

        // DONE []
        public IActionResult All(int id)
        {
            var viewModel = new AllApiaryHelpersViewModel
            {
                AllHelpers = this.apiaryHelperService.GetAllApiaryHelpersByApiaryId<ApiaryHelperViewModel>(id),
                ApiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(id),
                ApiaryId = id,
            };

            return this.View(viewModel);
        }

        // DONE []
        [HttpPost]
        public async Task<IActionResult> Delete(string userId, int apiaryId)
        {
            await this.apiaryHelperService.DeleteAsync(userId, apiaryId);
            // var apiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(apiaryId);

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно изтрит помощник!";
            return this.Redirect($"/ApiaryHelper/All/{apiaryId}");
        }

        // DONE []
        public IActionResult Edit(string userId, int apiaryId)
        {
            var inputModel = this.apiaryHelperService.GetApiaryHelper<EditApiaryHelperInputModel>(userId, apiaryId);
            inputModel.ApiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(apiaryId);
            return this.View(inputModel);
        }

        // DONE []
        [HttpPost]
        public async Task<IActionResult> Edit(EditApiaryHelperInputModel inputModel, string userId, int apiaryId)
        {
            if (!this.ModelState.IsValid)
            {
                var user = await this.userManager.FindByIdAsync(userId);
                inputModel.UserUserName = user.UserName;
                inputModel.ApiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(apiaryId);
                inputModel.ApiaryId = apiaryId;
                return this.View(inputModel);
            }

            await this.apiaryHelperService.EditAsync(userId, apiaryId, inputModel.Access);

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно редактиран помощник!";
            return this.Redirect($"/ApiaryHelper/All/{apiaryId}");
        }
    }
}
