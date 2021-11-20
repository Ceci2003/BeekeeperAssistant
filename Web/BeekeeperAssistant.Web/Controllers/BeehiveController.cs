namespace BeekeeperAssistant.Web.Controllers
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

        public async Task<IActionResult> All(int page = 1)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var allBehhives = this.beehiveService.GetAllUserBeehives<BeehiveViewModel>(currentUser.Id, GlobalConstants.BeehivesPerPage, (page - 1) * GlobalConstants.BeehivesPerPage);

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

        public async Task<IActionResult> ById(
            int beehiveId,
            string tabPage,
            int pageAllHarvests = 1,
            int pageAllTreatments = 1,
            int pageAllInspections = 1)
        {
            var viewModel = this.beehiveService.GetBeehiveById<BeehiveDataViewModel>(beehiveId);
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var apiaryId = this.apiaryService.GetApiaryIdByBeehiveId(beehiveId);

            if (viewModel.CreatorId != currentUser.Id &&
                !this.apiaryHelperService.IsApiaryHelper(currentUser.Id, apiaryId) &&
                !this.apiaryService.IsApiaryCreator(currentUser.Id, apiaryId) &&
                !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return this.BadRequest();
            }

            viewModel.BeehiveAccess =
                currentUser.Id == viewModel.CreatorId ||
                await this.userManager.IsInRoleAsync(currentUser, GlobalConstants.AdministratorRoleName) ?
                Access.ReadWrite :
                this.beehiveHelperService.GetUserBeehiveAccess(currentUser.Id, viewModel.Id);

            if (viewModel.Queen != null)
            {
                viewModel.QueenAccess = currentUser.Id == viewModel.CreatorId ||
                    await this.userManager.IsInRoleAsync(currentUser, GlobalConstants.AdministratorRoleName) ? Access.ReadWrite :
                    this.queenHelperService.GetUserQueenAccess(currentUser.Id, viewModel.QueenId);
            }

            // ----------------------------------
            viewModel.Harvests = new AllHarvestsViewModel();

            var allHarvestsCount = this.harvestService.GetAllBeehiveHarvestsCountByBeehiveId(beehiveId);
            var pagesHarvestsCount = (int)Math.Ceiling((double)allHarvestsCount / GlobalConstants.ApiariesPerPage);

            if (pageAllHarvests <= 0)
            {
                pageAllHarvests = 1;
            }
            else if (pageAllHarvests > pagesHarvestsCount)
            {
                pageAllHarvests = pagesHarvestsCount == 0 ? 1 : pagesHarvestsCount;
            }

            viewModel.Harvests.PagesCount = pagesHarvestsCount;

            if (viewModel.Harvests.PagesCount == 0)
            {
                viewModel.Harvests.PagesCount = 1;
            }

            viewModel.Harvests.CurrentPage = pageAllHarvests;

            var harvests = this.harvestService.GetAllBeehiveHarvests<HarvestDatavVewModel>(beehiveId, GlobalConstants.ApiariesPerPage, (pageAllHarvests - 1) * GlobalConstants.ApiariesPerPage);
            viewModel.Harvests.AllHarvests = harvests;

            // ----------------------------------
            viewModel.Treatments = new AllTreatementsViewModel();

            var allTreatemetsCount = this.treatmentService.GetBeehiveTreatmentsCountByBeehiveId(beehiveId);
            var pagesTreatementsCount = (int)Math.Ceiling((double)allTreatemetsCount / GlobalConstants.ApiariesPerPage);

            if (pageAllTreatments <= 0)
            {
                pageAllTreatments = 1;
            }
            else if (pageAllTreatments > pagesTreatementsCount)
            {
                pageAllTreatments = pagesTreatementsCount == 0 ? 1 : pagesTreatementsCount;
            }

            viewModel.Treatments.PagesCount = pagesTreatementsCount;

            if (viewModel.Treatments.PagesCount == 0)
            {
                viewModel.Treatments.PagesCount = 1;
            }

            viewModel.Treatments.CurrentPage = pageAllTreatments;

            var treatments = this.treatmentService.GetAllBeehiveTreatments<TreatmentDataViewModel>(beehiveId, GlobalConstants.ApiariesPerPage, (pageAllTreatments - 1) * GlobalConstants.ApiariesPerPage);
            viewModel.Treatments.AllTreatements = treatments;

            // ----------------------------------
            viewModel.Inspections = new AllInspectionsViewModel();

            var allinspectionsCount = this.inspectionService.GetAllBeehiveInspectionsCountByBeehiveId(beehiveId);
            var pageInspectionsCount = (int)Math.Ceiling((double)allinspectionsCount / GlobalConstants.ApiariesPerPage);

            if (pageAllInspections <= 0)
            {
                pageAllInspections = 1;
            }
            else if (pageAllInspections > pageInspectionsCount)
            {
                pageAllInspections = pageInspectionsCount == 0 ? 1 : pageInspectionsCount;
            }

            viewModel.Inspections.PagesCount = pageInspectionsCount;

            if (viewModel.Inspections.PagesCount == 0)
            {
                viewModel.Inspections.PagesCount = 1;
            }

            viewModel.Inspections.CurrentPage = pageAllInspections;

            var inspections = this.inspectionService.GetAllBeehiveInspections<InspectionDataViewModel>(beehiveId, GlobalConstants.ApiariesPerPage, (pageAllInspections - 1) * GlobalConstants.ApiariesPerPage);
            viewModel.Inspections.AllInspections = inspections;

            // ----------------------------------
            if (string.IsNullOrEmpty(tabPage))
            {
                tabPage = "Beehive";
            }

            viewModel.TabPage = tabPage;

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

        // DONE []
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
            return this.Redirect($"/Beehive/{apiaryNumber}/{beehiveId}");
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
            return this.Redirect($"/Beehive/{apiaryNumber}/{beehiveId}");
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
            return this.RedirectToAction("ByNumber", "Apiary", new { apiaryNumber = apiaryNumber, tabPage = "Beehives" });
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
