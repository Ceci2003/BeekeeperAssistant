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
            var viewModel = apiaryDiaryService.GetApiaryDiaryByApiaryId<ByApiaryIdApiaryDiaryViewModel>(id);

            if (viewModel == null)
            {
                viewModel = new ByApiaryIdApiaryDiaryViewModel();
                var apiary = apiaryService.GetApiaryById<ApiaryDataModel>(id);
                viewModel.ApiaryId = id;
                viewModel.ApiaryNumber = apiary.Number;
                viewModel.ApiaryName = apiary.Name;
                viewModel.ApiaryApiaryType = apiary.ApiaryType;
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save(int id, ByApiaryIdApiaryDiaryViewModel inputModel)
        {
            var currentUser = await userManager.GetUserAsync(User);

            var apiaryId = (int)default;

            if (apiaryService.HasDiary(id))
            {
                apiaryId = await apiaryDiaryService.SaveAsync(id, inputModel.Content, currentUser.Id);
            }
            else
            {
                apiaryId = await apiaryDiaryService.CreateAsync(id, inputModel.Content, currentUser.Id);
            }

            return RedirectToAction(nameof(this.ByApiaryId), new { id = apiaryId });
        }
    }
}
