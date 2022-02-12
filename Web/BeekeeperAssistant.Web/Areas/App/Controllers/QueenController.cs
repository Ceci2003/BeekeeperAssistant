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
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using BeekeeperAssistant.Web.ViewModels.Queens;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class QueenController : AppBaseController
    {
        private readonly IQueenService queenService;
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;
        private readonly IQueenHelperService queenHelperService;
        private readonly IBeehiveHelperService beehiveHelperService;
        private readonly UserManager<ApplicationUser> userManager;

        public QueenController(
            IQueenService queenService,
            IApiaryService apiaryService,
            IBeehiveService beehiveService,
            IQueenHelperService queenHelperService,
            IBeehiveHelperService beehiveHelperService,
            UserManager<ApplicationUser> userManager)
        {
            this.queenService = queenService;
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
            this.queenHelperService = queenHelperService;
            this.beehiveHelperService = beehiveHelperService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All(int page = 1)
        {
            if (page <= 0)
            {
                page = 1;
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = new AllQueenViewModel
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
            var viewModel = this.queenService.GetQueenByBeehiveId<ByBeehiveIdQueenViewModel>(id);

            if (viewModel == null)
            {
                var beehive = this.beehiveService.GetBeehiveById<BeehiveDataModel>(id);

                viewModel = new ByBeehiveIdQueenViewModel
                {
                    BeehiveNumber = beehive.Number,
                    BeehiveId = id,
                    BeehiveApiaryId = beehive.Apiary.Id,
                    BeehiveApiaryName = beehive.Apiary.Name,
                    BeehiveApiaryNumber = beehive.Apiary.Number,
                };

                return this.View(viewModel);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            viewModel.QueenAccess = await this.queenHelperService.GetUserQueenAccessAsync(currentUser.Id, viewModel.Id);

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

            var apiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(id);
            var beehiveNumber = this.beehiveService.GetBeehiveNumberById(id);

            inputModel.ApiaryNumber = apiaryNumber;
            inputModel.BeehiveNumber = beehiveNumber;

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateQueenInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var apiaryId = this.apiaryService.GetApiaryIdByBeehiveId(inputModel.BeehiveId);
            var apiaryOwner = this.apiaryService.GetApiaryOwnerIdByApiaryId(apiaryId);
            var beehiveId = await this.queenService
                .CreateUserQueenAsync(
                apiaryOwner,
                currentUser.Id,
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
            return this.RedirectToAction(nameof(this.ByBeehiveId), new { id = beehiveId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var beehiveId = await this.queenService.DeleteQueenAsync(id);

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно изтрита майка!";
            return this.RedirectToAction(nameof(this.ByBeehiveId), new { id = beehiveId });
        }

        public IActionResult Edit(int id)
        {
            var inputModel = this.queenService.GetQueenById<EditQueenInputModel>(id);

            var beehiveId = this.beehiveService.GetBeehiveIdByQueen(id);

            var apiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(beehiveId);
            var beehiveNumber = this.beehiveService.GetBeehiveNumberById(beehiveId);

            inputModel.BeehiveId = beehiveId;
            inputModel.ApiaryNumber = apiaryNumber;
            inputModel.BeehiveNumber = beehiveNumber;

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditQueenInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var beehiveId = await this.queenService.EditQueenAsync(id, inputModel.FertilizationDate, inputModel.GivingDate, inputModel.QueenType, inputModel.Origin, inputModel.HygenicHabits, inputModel.Temperament, inputModel.Color, inputModel.Breed);

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно редактирана майка!";
            return this.RedirectToAction(nameof(this.ByBeehiveId), new { id = beehiveId });
        }

        public async Task<IActionResult> Bookmark(int id)
        {
            await this.queenService.BookmarkQueenAsync(id);
            return this.RedirectToAction(nameof(this.All));
        }
    }
}
