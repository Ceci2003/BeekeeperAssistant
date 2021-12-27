﻿namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using BeekeeperAssistant.Web.ViewModels.Harvest;
    using BeekeeperAssistant.Web.ViewModels.Inspection;
    using BeekeeperAssistant.Web.ViewModels.Treatments;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class BeehiveController : BaseController
    {
        private readonly IApiaryService apiaryService;
        private readonly IApiaryHelperService apiaryHelperService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBeehiveHelperService beehiveHelperService;
        private readonly IBeehiveService beehiveService;
        private readonly IQueenService queenService;
        private readonly IHarvestService harvestService;
        private readonly ITreatmentService treatmentService;
        private readonly IInspectionService inspectionService;
        private readonly IQueenHelperService queenHelperService;
        private readonly IExcelExportService excelExportService;

        public BeehiveController(
            IApiaryService apiaryService,
            IApiaryHelperService apiaryHelperService,
            UserManager<ApplicationUser> userManager,
            IBeehiveHelperService beehiveHelperService,
            IBeehiveService beehiveService,
            IQueenService queenService,
            IHarvestService harvestService,
            ITreatmentService treatmentService,
            IInspectionService inspectionService,
            IQueenHelperService queenHelperService,
            IExcelExportService excelExportService)
        {
            this.apiaryService = apiaryService;
            this.apiaryHelperService = apiaryHelperService;
            this.userManager = userManager;
            this.beehiveHelperService = beehiveHelperService;
            this.beehiveService = beehiveService;
            this.queenService = queenService;
            this.harvestService = harvestService;
            this.treatmentService = treatmentService;
            this.inspectionService = inspectionService;
            this.queenHelperService = queenHelperService;
            this.excelExportService = excelExportService;
        }

        public async Task<IActionResult> All(int page = 1, string orderBy = null)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var allBehhives = this.beehiveService.GetAllUserBeehives<BeehiveViewModel>(currentUser.Id, GlobalConstants.BeehivesPerPage, (page - 1) * GlobalConstants.BeehivesPerPage, orderBy);

            var viewModel = new AllBeehivesViewModel
            {
                AllBeehives = allBehhives,
            };

            var count = this.beehiveService.GetAllUserBeehivesCount(currentUser.Id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / GlobalConstants.BeehivesPerPage);

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        public async Task<IActionResult> AllByApiaryId(int id, int page = 1)
        {
            var viewModel = this.apiaryService.GetApiaryById<AllBeehivesByApiaryIdViewModel>(id);
            var currentUser = await this.userManager.GetUserAsync(this.User);

            viewModel.AllBeehives = this.beehiveService.GetBeehivesByApiaryId<BeehiveByApiaryIdViewModel>(id, GlobalConstants.BeehivesPerPage, (page - 1) * GlobalConstants.BeehivesPerPage);
            viewModel.ApiaryAccess = await this.apiaryHelperService.GetUserApiaryAccessAsync(currentUser.Id, id);

            foreach (var beehive in viewModel.AllBeehives)
            {
                beehive.BeehiveAccess = await this.beehiveHelperService.GetUserBeehiveAccessAsync(currentUser.Id, beehive.Id);
            }

            var count = this.beehiveService.GetAllBeehivesCountByApiaryId(id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / GlobalConstants.BeehivesPerPage);

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        public async Task<IActionResult> ById(int id)
        {
            var viewModel = this.beehiveService.GetBeehiveById<BeehiveDataViewModel>(id);

            var currentUser = await this.userManager.GetUserAsync(this.User);

            viewModel.BeehiveAccess = await this.beehiveHelperService.GetUserBeehiveAccessAsync(currentUser.Id, id);

            return this.View(viewModel);
        }

        // DONE []
        public async Task<IActionResult> Create(int? id)
        {
            var inputModel = new CreateBeehiveInputModel();

            if (id == null)
            {
                var currentUser = await this.userManager.GetUserAsync(this.User);
                inputModel.AllApiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
            }
            else
            {
                inputModel.ApiaryId = id.Value;
            }

            inputModel.Date = DateTime.UtcNow.Date;

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, CreateBeehiveInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                if (!id.HasValue)
                {
                    inputModel.AllApiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
                }

                return this.View(inputModel);
            }

            var apiaryCreatorId = this.apiaryService.GetApiaryCreatorIdByApiaryId(inputModel.ApiaryId);

            var beehiveId = await this.beehiveService
                .CreateUserBeehiveAsync(
                apiaryCreatorId,
                inputModel.Number,
                inputModel.BeehiveSystem,
                inputModel.BeehiveType,
                inputModel.Date,
                inputModel.ApiaryId,
                inputModel.BeehivePower,
                inputModel.HasDevice,
                inputModel.HasPolenCatcher,
                inputModel.HasPropolisCatcher,
                inputModel.IsItMovable);

            var apiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(beehiveId);
            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно създаден кошер!";
            return this.RedirectToAction("ById", "Beehive", new { id = beehiveId });
        }

        // DONE []
        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var inputModel = this.beehiveService.GetBeehiveById<EditBeehiveInputModel>(id);
            inputModel.AllApiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);

            return this.View(inputModel);
        }

        // DONE []
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditBeehiveInputModel inputModel)
        {
            var beehiveId = await this.beehiveService.EditUserBeehiveAsync(
                id,
                inputModel.Number,
                inputModel.BeehiveSystem,
                inputModel.BeehiveType,
                inputModel.Date,
                inputModel.ApiaryId,
                inputModel.BeehivePower,
                inputModel.HasDevice,
                inputModel.HasPolenCatcher,
                inputModel.HasPropolisCatcher,
                inputModel.IsItMovable);

            var apiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(beehiveId);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно редактиран кошер!";
            return this.RedirectToAction("ById", "Beehive", new { id = beehiveId });
        }

        // DONE []
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataViewModel>(id);
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var apiaryId = this.apiaryService.GetApiaryIdByBeehiveId(beehive.Id);

            if (beehive.CreatorId != currentUser.Id &&
                !this.beehiveHelperService.IsBeehiveHelper(currentUser.Id, beehive.Id) &&
                !this.apiaryService.IsApiaryCreator(currentUser.Id, apiaryId) &&
                !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.BadRequest();
            }

            var apiaryNumber = await this.beehiveService.DeleteBeehiveByIdAsync(id);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно изтрит кошер!";
            return this.RedirectToAction("ByNumber", "Apiary", new { apiaryNumber = apiaryNumber});
        }

        // DONE []
        public async Task<IActionResult> ExportToExcel(int? id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var result = this.excelExportService.ExportAsExcelBeehive(currentUser.Id, id);

            this.Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            return new FileContentResult(result.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        // DONE []
        public async Task<IActionResult> Bookmark(int id, string returnUrl)
        {
            var apiaryId = await this.beehiveService.BookmarkBeehiveAsync(id);

            if (!apiaryId.HasValue)
            {
                return this.BadRequest();
            }

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
