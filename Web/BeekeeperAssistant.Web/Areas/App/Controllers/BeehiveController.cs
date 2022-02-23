namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.BeehiveMarkFlags;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using BeekeeperAssistant.Web.ViewModels.Harvest;
    using BeekeeperAssistant.Web.ViewModels.Inspections;
    using BeekeeperAssistant.Web.ViewModels.Treatments;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class BeehiveController : AppBaseController
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
        private readonly ITemporaryApiaryBeehiveService temporaryApiaryBeehiveService;
        private readonly IBeehiveMarkFlagService beehiveMarkFlagService;

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
            IExcelExportService excelExportService,
            ITemporaryApiaryBeehiveService temporaryApiaryBeehiveService,
            IBeehiveMarkFlagService beehiveMarkFlagService)
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
            this.temporaryApiaryBeehiveService = temporaryApiaryBeehiveService;
            this.beehiveMarkFlagService = beehiveMarkFlagService;
        }

        public async Task<IActionResult> All(int page = 1, string orderBy = null)
        {
            if (page <= 0)
            {
                page = 1;
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var allBehhives = this.beehiveService.GetAllUserBeehives<BeehiveDataModel>(currentUser.Id, GlobalConstants.BeehivesPerPage, (page - 1) * GlobalConstants.BeehivesPerPage, orderBy);

            foreach (var beehive in allBehhives)
            {
                beehive.MarkFlagType = this.beehiveMarkFlagService.GetBeehiveFlagTypeByBeehiveId(beehive.Id);
            }

            var viewModel = new AllBeehiveViewModel
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
            if (page <= 0)
            {
                page = 1;
            }

            var viewModel = this.apiaryService.GetApiaryById<AllByApiaryIdBeehiveViewModel>(id);
            var currentUser = await this.userManager.GetUserAsync(this.User);

            viewModel.AllBeehives = this.beehiveService.GetBeehivesByApiaryId<ByApiaryIdBeehiveViewModel>(id, GlobalConstants.BeehivesPerPage, (page - 1) * GlobalConstants.BeehivesPerPage);
            viewModel.ApiaryAccess = await this.apiaryHelperService.GetUserApiaryAccessAsync(currentUser.Id, id);

            foreach (var beehive in viewModel.AllBeehives)
            {
                beehive.BeehiveAccess = await this.beehiveHelperService.GetUserBeehiveAccessAsync(currentUser.Id, beehive.Id);
                beehive.MarkFlagType = this.beehiveMarkFlagService.GetBeehiveFlagTypeByBeehiveId(beehive.Id);
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

        public async Task<IActionResult> AllByMovableApiaryId(int id, int page = 1)
        {
            if (page <= 0)
            {
                page = 1;
            }

            var viewModel = this.apiaryService.GetApiaryById<AllByMovableApiaryIdBeehiveViewModel>(id);
            var currentUser = await this.userManager.GetUserAsync(this.User);

            viewModel.AllBeehives = this.temporaryApiaryBeehiveService.GetBeehivesByApiaryId<ByMovableApiaryIdBeehiveViewModel>(id, GlobalConstants.BeehivesPerPage, (page - 1) * GlobalConstants.BeehivesPerPage);

            viewModel.ApiaryAccess = await this.apiaryHelperService.GetUserApiaryAccessAsync(currentUser.Id, id);

            foreach (var beehive in viewModel.AllBeehives)
            {
                beehive.BeehiveBeehiveAccess = await this.beehiveHelperService.GetUserBeehiveAccessAsync(currentUser.Id, beehive.BeehiveId);
                beehive.MarkFlagType = this.beehiveMarkFlagService.GetBeehiveFlagTypeByBeehiveId(beehive.BeehiveId);
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
            var viewModel = this.beehiveService.GetBeehiveById<ByIdBeehiveViewModel>(id);

            var hasFlag = this.beehiveMarkFlagService.BeehiveHasFlag(id);
            if (hasFlag)
            {
                viewModel.HasFlag = hasFlag;
                viewModel.FlagViewModel = this.beehiveMarkFlagService.GetBeehiveFlagByBeehiveId<BeehivemarkFlagViewModel>(id);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            viewModel.BeehiveAccess = await this.beehiveHelperService.GetUserBeehiveAccessAsync(currentUser.Id, id);

            return this.View(viewModel);
        }

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

            var apiaryOwnerId = this.apiaryService.GetApiaryOwnerIdByApiaryId(inputModel.ApiaryId);

            var beehiveId = await this.beehiveService
                .CreateBeehiveAsync(
                apiaryOwnerId,
                currentUser.Id,
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

            this.apiaryService.GetApiaryNumberByBeehiveId(beehiveId);
            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно създаден кошер!";

            return this.RedirectToAction(nameof(this.CreateRedirect), new { beehiveId = beehiveId, apiaryId = inputModel.ApiaryId });
        }

        public IActionResult CreateRedirect(int beehiveId, int apiaryId)
        {
            var viewModel = new CreateRedirectBeehiveViewModel
            {
                ApiaryId = apiaryId,
                BeehiveId = beehiveId,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var inputModel = this.beehiveService.GetBeehiveById<EditBeehiveInputModel>(id);
            inputModel.AllApiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);

            return this.View(inputModel);
        }

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

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно редактиран кошер!";
            return this.RedirectToAction(nameof(this.ById), new { id = beehiveId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var beehive = this.beehiveService.GetBeehiveById<ByIdBeehiveViewModel>(id);
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var apiaryId = this.apiaryService.GetApiaryIdByBeehiveId(beehive.Id);

            if (beehive.CreatorId != currentUser.Id &&
                !this.beehiveHelperService.IsBeehiveHelper(currentUser.Id, beehive.Id) &&
                !this.apiaryService.IsApiaryCreator(currentUser.Id, apiaryId) &&
                !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.BadRequest();
            }

            await this.beehiveService.DeleteBeehiveByIdAsync(id);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно изтрит кошер!";
            return this.RedirectToAction(nameof(this.AllByApiaryId), new { id = apiaryId });
        }

        public async Task<IActionResult> ExportToExcel(int? id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var result = this.excelExportService.ExportAsExcelBeehive(currentUser.Id, id);

            this.Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            return new FileContentResult(result.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

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

        public async Task<IActionResult> SelectApiaryToMoveBeehive(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var inputModel = new SelectApiaryToMoveBeehiveIn
            {
                BeehiveId = id,
                BeehiveNumber = this.beehiveService.GetBeehiveNumberById(id),
                BeehiveApiaryId = this.apiaryService.GetApiaryIdByBeehiveId(id),
                BeehiveApiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(id),
                BeehiveApiaryName = this.apiaryService.GetApiaryNameByBeehiveId(id),
                AllApiaries = this.apiaryService.GetUserApiariesWithoutTemporaryAsKeyValuePairs(currentUser.Id),
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> SelectApiaryToMoveBeehive(int id, SelectApiaryToMoveBeehiveIn inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                inputModel.AllApiaries = this.apiaryService.GetUserApiariesWithoutTemporaryAsKeyValuePairs(currentUser.Id);
                return this.View(inputModel);
            }

            if (this.beehiveService.BeehiveNumberExistsInApiary(inputModel.BeehiveNumber, inputModel.SelectedApiaryId))
            {
                return this.RedirectToAction(nameof(this.ChooseNewNumberForBeehive), new { id = id, selectedApiaryId = inputModel.SelectedApiaryId });
            }

            await this.beehiveService.UpdateBeehiveApiary(id, inputModel.SelectedApiaryId);

            var messageText = $"Успешно преместихте кошер №{inputModel.BeehiveNumber}!";

            this.TempData[GlobalConstants.SuccessMessage] = messageText;
            return this.RedirectToAction(nameof(this.ById), new { id = id });
        }

        public async Task<IActionResult> ChooseNewNumberForBeehive(int id, int selectedApiaryId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var inputModel = new ChooseNewNumberForBeehiveInputModel
            {
                BeehiveId = id,
                BeehiveNumber = this.beehiveService.GetBeehiveNumberById(id),
                BeehiveApiaryId = selectedApiaryId,
                BeehiveApiaryName = this.apiaryService.GetApiaryNameByApiaryId(selectedApiaryId),
                BeehiveApiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(selectedApiaryId),
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChooseNewNumberForBeehive(int id, ChooseNewNumberForBeehiveInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.beehiveService.UpdateBeehiveNumberAndApiary(id, inputModel.BeehiveNumber, inputModel.BeehiveApiaryId);

            var messageText = $"Успешно преместихте кошер №{inputModel.BeehiveNumber}!";

            this.TempData[GlobalConstants.SuccessMessage] = messageText;
            return this.RedirectToAction(nameof(this.ById), new { id = id });
        }
    }
}
