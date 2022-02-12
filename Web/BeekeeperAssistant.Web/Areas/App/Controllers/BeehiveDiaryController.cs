using BeekeeperAssistant.Data.Models;
using BeekeeperAssistant.Services.Data;
using BeekeeperAssistant.Web.ViewModels.ApiaryDiaries;
using BeekeeperAssistant.Web.ViewModels.BeehiveDiaries;
using BeekeeperAssistant.Web.ViewModels.Beehives;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    public class BeehiveDiaryController : AppBaseController
    {
        private readonly IBeehiveDiaryService beehiveDiaryService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBeehiveService beehiveService;

        public BeehiveDiaryController(
            IBeehiveDiaryService beehiveDiaryService,
            UserManager<ApplicationUser> userManager,
            IBeehiveService beehiveService)
        {
            this.beehiveDiaryService = beehiveDiaryService;
            this.userManager = userManager;
            this.beehiveService = beehiveService;
        }

        public IActionResult ByBeehiveId(int id)
        {
            var viewModel = this.beehiveDiaryService.GetBeehiveDiaryByBeehiveId<ByBeehiveIdBeehiveDiaryViewModel>(id);

            if (viewModel == null)
            {
                viewModel = new ByBeehiveIdBeehiveDiaryViewModel();

                var beehive = this.beehiveService.GetBeehiveById<BeehiveDataModel>(id);

                viewModel.BeehiveId = id;
                viewModel.BeehiveApiaryNumber = beehive.Apiary.Number;
                viewModel.BeehiveApiaryName = beehive.Apiary.Name;
                viewModel.BeehiveNumber = beehive.Number;
                viewModel.BeehiveApiaryId = beehive.Apiary.Id;
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save(int id, ByBeehiveIdBeehiveDiaryViewModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var apiaryId = (int)default;

            if (this.beehiveService.HasDiary(id))
            {
                apiaryId = await this.beehiveDiaryService.SaveAsync(id, inputModel.Content, currentUser.Id);
            }
            else
            {
                apiaryId = await this.beehiveDiaryService.CreateAsync(id, inputModel.Content, currentUser.Id);
            }

            return this.RedirectToAction(nameof(this.ByBeehiveId), new { id = apiaryId });
        }
    }
}
