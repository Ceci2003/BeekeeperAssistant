namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.ApiaryDiaries;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ApiaryDiaryController : AppBaseController
    {
        private readonly IApiaryDiaryService apiaryDiaryService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApiaryService apiaryService;

        public ApiaryDiaryController(
            IApiaryDiaryService apiaryDiaryService,
            UserManager<ApplicationUser> userManager,
            IApiaryService apiaryService)
        {
            this.apiaryDiaryService = apiaryDiaryService;
            this.userManager = userManager;
            this.apiaryService = apiaryService;
        }

        public IActionResult ByApiaryId(int id)
        {
            var viewModel = this.apiaryDiaryService.GetApiaryDiaryByApiaryId<ByApiaryIdApiaryDiaryViewModel>(id);

            if (viewModel == null)
            {
                viewModel = new ByApiaryIdApiaryDiaryViewModel();
                var apiary = this.apiaryService.GetApiaryById<ApiaryDataModel>(id);
                viewModel.ApiaryId = id;
                viewModel.ApiaryNumber = apiary.Number;
                viewModel.ApiaryName = apiary.Name;
                viewModel.ApiaryApiaryType = apiary.ApiaryType;
            }

            this.TempData.Keep();

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save(int id, ByApiaryIdApiaryDiaryViewModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var apiaryId = (int)default;

            if (this.apiaryService.HasDiary(id))
            {
                apiaryId = await this.apiaryDiaryService.SaveAsync(id, inputModel.Content, currentUser.Id);
            }
            else
            {
                apiaryId = await this.apiaryDiaryService.CreateAsync(id, inputModel.Content, currentUser.Id);
            }

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно запазихте дневника на пчелина!";

            return this.RedirectToAction(nameof(this.ByApiaryId), new { id = apiaryId });
        }
    }
}
