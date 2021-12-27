namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using BeekeeperAssistant.Web.ViewModels.Queens;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class QueenController : Controller
    {
        private readonly IQueenService queenService;
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;
        private readonly IQueenHelperService queenHelperService;
        private readonly UserManager<ApplicationUser> userManager;

        public QueenController(
            IQueenService queenService,
            IApiaryService apiaryService,
            IBeehiveService beehiveService,
            IQueenHelperService queenHelperService,
            UserManager<ApplicationUser> userManager)
        {
            this.queenService = queenService;
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
            this.queenHelperService = queenHelperService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All(int page = 1)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = new AllQueensViewModel
            {
                AllQueens = this.queenService.GetAllUserQueens<QueenViewModel>(user.Id, GlobalConstants.QueensPerPage, (page - 1) * GlobalConstants.QueensPerPage),
            };

            var count = this.queenService.GetAllUserQueensCount(user.Id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / GlobalConstants.QueensPerPage);
            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        public async Task<IActionResult> ByBeehiveId(int id)
        {
            var viewModel = this.queenService.GetQueenByBeehiveId<QueenDataViewModel>(id);

            if (viewModel == null)
            {
                viewModel = new QueenDataViewModel
                {
                    BeehiveNumber = this.beehiveService.GetBeehiveNumberById(id),
                    BeehiveId = id,
                };

                return this.View(viewModel);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            viewModel.BeehiveAccess = await this.queenHelperService.GetUserQueenAccessAsync(currentUser.Id, id);

            return this.View(viewModel);
        }

        public IActionResult Create(int id)
        {
            var inputModel = new CreateQueenInputModel
            {
                BeehiveId = id,
                GivingDate = DateTime.UtcNow.Date,
                FertilizationDate = DateTime.UtcNow.Date,
            };

            var apiary = this.apiaryService.GetUserApiaryByBeehiveId<ApiaryViewModel>(id);
            var beehive = this.beehiveService.GetBeehiveById<BeehiveViewModel>(id);

            inputModel.ApiaryNumber = apiary.Number;
            inputModel.BeehiveNumber = beehive.Number;

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateQueenInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var beehiveId = await this.queenService
                .CreateUserQueenAsync(
                user.Id,
                inputModel.BeehiveId,
                inputModel.FertilizationDate,
                inputModel.GivingDate,
                inputModel.QueenType,
                inputModel.Origin,
                inputModel.HygenicHabits,
                inputModel.Temperament,
                inputModel.Color,
                inputModel.Breed);

            var apiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(beehiveId);

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно създадена майка!";
            return this.RedirectToAction("ByBeehiveId", "Queen", new { id = beehiveId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var beehiveId = await this.queenService.DeleteQueenAsync(id);

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно изтрита майка!";
            return this.RedirectToAction("ByBeehiveId", "Queen", new { id = beehiveId });
        }

        public IActionResult Edit(int id, int beehiveId)
        {
            var inputModel = this.queenService.GetQueenById<EditQueenInutModel>(id);

            var apiary = this.apiaryService.GetUserApiaryByBeehiveId<ApiaryViewModel>(beehiveId);
            var beehive = this.beehiveService.GetBeehiveById<BeehiveViewModel>(beehiveId);

            inputModel.BeehiveId = beehiveId;
            inputModel.ApiaryNumber = apiary.Number;
            inputModel.BeehiveNumber = beehive.Number;

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditQueenInutModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var beehiveId = await this.queenService.EditQueenAsync(id, inputModel.FertilizationDate, inputModel.GivingDate, inputModel.QueenType, inputModel.Origin, inputModel.HygenicHabits, inputModel.Temperament, inputModel.Color, inputModel.Breed);

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно редактирана майка!";
            return this.RedirectToAction("ByBeehiveId", "Queen", new { id = beehiveId });
        }

        public async Task<IActionResult> Bookmark(int id)
        {
            await this.queenService.BookmarkQueenAsync(id);
            return this.Redirect("/Queen/All");
        }
    }
}
