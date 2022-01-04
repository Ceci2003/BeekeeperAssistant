namespace BeekeeperAssistant.Web.Areas.App.Controllers
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

    public class ApiaryHelperController : AppBaseController
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
            var apiaryNumber = apiaryService.GetApiaryNumberByApiaryId(id);
            var viewModel = new AddApiaryHelperInputModel
            {
                ApiaryNumber = apiaryNumber,
                ApiaryId = id,
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int id, AddApiaryHelperInputModel viewModel)
        {
            var currentUser = await userManager.GetUserAsync(User);

            viewModel.ApiaryId = id;
            viewModel.ApiaryNumber = apiaryService.GetApiaryNumberByApiaryId(id);

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if (currentUser.UserName == viewModel.UserName)
            {
                ModelState.AddModelError("UserName", "Не може да добавите себе си!");
                return View(viewModel);
            }

            var user = await userManager.FindByNameAsync(viewModel.UserName);
            if (apiaryHelperService.IsApiaryHelper(user.Id, id))
            {
                ModelState.AddModelError("UserName", "Потребителят вече е помощник!");
                return View(viewModel);
            }

            await apiaryHelperService.AddAsync(user.Id, id);

            var helper = await userManager.FindByNameAsync(viewModel.UserName);
            await emailSender.SendEmailAsync(
                  configuration["SendGrid:RecipientEmail"],
                  GlobalConstants.SystemName,
                  currentUser.Email,
                  "Успешно добавен помощник",
                  $"Успешно добавихте <strong>{helper.UserName}</strong>, като помощник на пчелин: <strong>{viewModel.ApiaryNumber}</strong>.");

            await emailSender.SendEmailAsync(
                  configuration["SendGrid:RecipientEmail"],
                  GlobalConstants.SystemName,
                  helper.Email,
                  "Успешно добавен помощник",
                  $"Успешно бяхте добавени, като помощник на пчелин: <strong>{viewModel.ApiaryNumber}</strong> от <strong>{currentUser.Email}</strong>.");

            TempData[GlobalConstants.SuccessMessage] = "Успешно добавен помощник!";
            return RedirectToAction(nameof(this.All), new { id = viewModel.ApiaryId });
        }

        public IActionResult All(int id)
        {
            var viewModel = new AllApiaryHelperViewModel
            {
                AllHelpers = apiaryHelperService.GetAllApiaryHelpersByApiaryId<ApiaryHelperViewModel>(id),
                ApiaryNumber = apiaryService.GetApiaryNumberByApiaryId(id),
                ApiaryId = id,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string userId, int apiaryId)
        {
            await apiaryHelperService.DeleteAsync(userId, apiaryId);

            TempData[GlobalConstants.SuccessMessage] = "Успешно изтрит помощник!";
            return RedirectToAction(nameof(this.All), new { id = apiaryId });
        }

        public IActionResult Edit(string userId, int apiaryId)
        {
            var inputModel = apiaryHelperService.GetApiaryHelper<EditApiaryHelperInputModel>(userId, apiaryId);
            inputModel.ApiaryNumber = apiaryService.GetApiaryNumberByApiaryId(apiaryId);
            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditApiaryHelperInputModel inputModel, string userId, int apiaryId)
        {
            if (!ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(userId);
                inputModel.UserUserName = user.UserName;
                inputModel.ApiaryNumber = apiaryService.GetApiaryNumberByApiaryId(apiaryId);
                inputModel.ApiaryId = apiaryId;
                return View(inputModel);
            }

            await apiaryHelperService.EditAsync(userId, apiaryId, inputModel.Access);

            TempData[GlobalConstants.SuccessMessage] = "Успешно редактиран помощник!";
            return RedirectToAction(nameof(this.All), new { id = apiaryId });
        }
    }
}
