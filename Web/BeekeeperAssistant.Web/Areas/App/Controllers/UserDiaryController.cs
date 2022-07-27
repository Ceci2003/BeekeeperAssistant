namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.UserDiary;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserDiaryController : AppBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserDiaryService userDiaryService;

        public UserDiaryController(
            UserManager<ApplicationUser> userManager,
            IUserDiaryService userDiaryService)
        {
            this.userManager = userManager;
            this.userDiaryService = userDiaryService;
        }

        public async Task<IActionResult> ByUserId()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var viewModel = this.userDiaryService.GetDiaryByUserId<ByUserIdViewModel>(currentUser.Id);

            if (viewModel == null)
            {
                viewModel = new ByUserIdViewModel();
                viewModel.UserId = currentUser.Id;
            }

            this.TempData[GlobalConstants.InfoMessage] = $"Моля не забравяйте да запазите дневника след като извършите промяна!";

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ByUserIdViewModel inputModel)
        {
            var diaryId = (int)default;

            if (this.userDiaryService.HasDiary(inputModel.UserId))
            {
                diaryId = await this.userDiaryService.SaveAsync(inputModel.UserId, inputModel.Content);
            }
            else
            {
                diaryId = await this.userDiaryService.CreateAsync(inputModel.Content, inputModel.UserId);
            }

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно запазихте промените в дневника!";

            return this.RedirectToAction(nameof(this.ByUserId));
        }
    }
}
