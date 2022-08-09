namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.UserTasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserTaskController : AppBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserTaskService userTaskService;

        public UserTaskController(
            UserManager<ApplicationUser> userManager,
            IUserTaskService userTaskService)
        {
            this.userManager = userManager;
            this.userTaskService = userTaskService;
        }

        public IActionResult All(int? editId)
        {
            var userId = this.userManager.GetUserId(this.User);
            var viewModel = new AllByUserIdUserTaskViewModel()
            {
                UserTasks = this.userTaskService.GetAllUserTasks<UserTaskViewModel>(userId),
                CreateModel = new CreateUserTaskInputModel()
                {
                    DisplayCss = true,
                },
            };

            if (editId.HasValue)
            {
                viewModel.EditModel = this.userTaskService.GetUserTaskById<EditUserTaskInputModel>(editId.Value);
                viewModel.CreateModel.DisplayCss = false;
            }

            this.TempData.Keep();

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserTaskInputModel inputModel)
        {
            var userId = this.userManager.GetUserId(this.User);

            if (string.IsNullOrWhiteSpace(inputModel.Color))
            {
                inputModel.Color = "#639CBF";
            }

            await this.userTaskService.CreateAsync(
                userId,
                inputModel.Title,
                inputModel.Content,
                inputModel.Color,
                inputModel.StartDate,
                inputModel.EndDate);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно добавихте задачата!";

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.userTaskService.DeleteAsync(id);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно маркирахте задачата като завършена!";

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserTaskInputModel inputModel)
        {
            await this.userTaskService.EditAsync(
                inputModel.Id,
                inputModel.Title,
                inputModel.Content,
                inputModel.Color,
                inputModel.StartDate,
                inputModel.EndDate);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно радактирахте задачата!";

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
