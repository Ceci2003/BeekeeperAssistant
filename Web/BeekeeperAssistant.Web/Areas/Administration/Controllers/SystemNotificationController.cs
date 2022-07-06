namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Administration.SystemNotification;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class SystemNotificationController : AdministrationController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISystemNotificationService systemNotificationService;

        public SystemNotificationController(
            UserManager<ApplicationUser> userManager,
            ISystemNotificationService systemNotificationService)
        {
            this.userManager = userManager;
            this.systemNotificationService = systemNotificationService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNotificationInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            await this.systemNotificationService.CreateAsync(
                inputModel.Title,
                inputModel.Content,
                inputModel.Version,
                inputModel.ImageUrl,
                currentUser.Id);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно добавено известие!";

            return this.RedirectToAction("AdditionalPage", "Home", new { Area = "App" });
        }
    }
}
