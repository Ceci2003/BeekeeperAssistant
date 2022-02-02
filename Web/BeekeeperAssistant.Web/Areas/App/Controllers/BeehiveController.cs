namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
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
            ITemporaryApiaryBeehiveService temporaryApiaryBeehiveService)
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
        }

        public async Task<IActionResult> All(int page = 1, string orderBy = null)
        {
            if (page <= 0)
            {
                page = 1;
            }

            var currentUser = await userManager.GetUserAsync(User);
            var allBehhives = beehiveService.GetAllUserBeehives<BeehiveDataModel>(currentUser.Id, GlobalConstants.BeehivesPerPage, (page - 1) * GlobalConstants.BeehivesPerPage, orderBy);

            var viewModel = new AllBeehiveViewModel
            {
                AllBeehives = allBehhives,
            };

            var count = beehiveService.GetAllUserBeehivesCount(currentUser.Id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / GlobalConstants.BeehivesPerPage);

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = page;

            return View(viewModel);
        }

        public async Task<IActionResult> AllByApiaryId(int id, int page = 1)
        {
            if (page <= 0)
            {
                page = 1;
            }

            var viewModel = apiaryService.GetApiaryById<AllByApiaryIdBeehiveViewModel>(id);
            var currentUser = await userManager.GetUserAsync(User);

            viewModel.AllBeehives = beehiveService.GetBeehivesByApiaryId<ByApiaryIdBeehiveViewModel>(id, GlobalConstants.BeehivesPerPage, (page - 1) * GlobalConstants.BeehivesPerPage);
            viewModel.ApiaryAccess = await apiaryHelperService.GetUserApiaryAccessAsync(currentUser.Id, id);

            foreach (var beehive in viewModel.AllBeehives)
            {
                beehive.BeehiveAccess = await beehiveHelperService.GetUserBeehiveAccessAsync(currentUser.Id, beehive.Id);
            }

            var count = beehiveService.GetAllBeehivesCountByApiaryId(id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / GlobalConstants.BeehivesPerPage);

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = page;

            return View(viewModel);
        }

        public async Task<IActionResult> AllByMovableApiaryId(int id, int page = 1)
        {
            if (page <= 0)
            {
                page = 1;
            }

            var viewModel = temporaryApiaryBeehiveService.GetApiaryById<AllByMovableApiaryIdBeehiveViewModel>(id);
            var currentUser = await userManager.GetUserAsync(User);

            viewModel.AllBeehives = temporaryApiaryBeehiveService.GetBeehivesByApiaryId<ByMovableApiaryIdBeehiveViewModel>(id, GlobalConstants.BeehivesPerPage, (page - 1) * GlobalConstants.BeehivesPerPage);

            viewModel.ApiaryAccess = await apiaryHelperService.GetUserApiaryAccessAsync(currentUser.Id, id);

            foreach (var beehive in viewModel.AllBeehives)
            {
                beehive.BeehiveBeehiveAccess = await beehiveHelperService.GetUserBeehiveAccessAsync(currentUser.Id, beehive.BeehiveId);
            }

            var count = beehiveService.GetAllBeehivesCountByApiaryId(id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / GlobalConstants.BeehivesPerPage);

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = page;

            return View(viewModel);
        }

        public async Task<IActionResult> ById(int id)
        {
            var viewModel = beehiveService.GetBeehiveById<ByIdBeehiveViewModel>(id);

            var currentUser = await userManager.GetUserAsync(User);

            viewModel.BeehiveAccess = await beehiveHelperService.GetUserBeehiveAccessAsync(currentUser.Id, id);

            return View(viewModel);
        }

        public async Task<IActionResult> Create(int? id, bool stayOnPage = false)
        {
            var inputModel = new CreateBeehiveInputModel();

            if (id == null)
            {
                var currentUser = await userManager.GetUserAsync(User);
                inputModel.AllApiaries = apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
            }
            else
            {
                inputModel.ApiaryId = id.Value;
            }

            inputModel.Date = DateTime.UtcNow.Date;
            inputModel.StayOnThePage = stayOnPage;

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, CreateBeehiveInputModel inputModel)
        {
            var currentUser = await userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                if (!id.HasValue)
                {
                    inputModel.AllApiaries = apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
                }

                return View(inputModel);
            }

            var apiaryOwnerId = apiaryService.GetApiaryOwnerIdByApiaryId(inputModel.ApiaryId);

            var beehiveId = await beehiveService
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

            apiaryService.GetApiaryNumberByBeehiveId(beehiveId);
            TempData[GlobalConstants.SuccessMessage] = $"Успешно създаден кошер!";

            if (inputModel.StayOnThePage)
            {
                inputModel.Number += 1;

                return View(inputModel);
            }

            return RedirectToAction(nameof(this.ById), new { id = beehiveId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = await userManager.GetUserAsync(User);

            var inputModel = beehiveService.GetBeehiveById<EditBeehiveInputModel>(id);
            inputModel.AllApiaries = apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditBeehiveInputModel inputModel)
        {
            var beehiveId = await beehiveService.EditUserBeehiveAsync(
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

            TempData[GlobalConstants.SuccessMessage] = $"Успешно редактиран кошер!";
            return RedirectToAction(nameof(this.ById), new { id = beehiveId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var beehive = beehiveService.GetBeehiveById<ByIdBeehiveViewModel>(id);
            var currentUser = await userManager.GetUserAsync(User);

            var apiaryId = apiaryService.GetApiaryIdByBeehiveId(beehive.Id);

            if (beehive.CreatorId != currentUser.Id &&
                !beehiveHelperService.IsBeehiveHelper(currentUser.Id, beehive.Id) &&
                !apiaryService.IsApiaryCreator(currentUser.Id, apiaryId) &&
                !User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return BadRequest();
            }

            await beehiveService.DeleteBeehiveByIdAsync(id);

            TempData[GlobalConstants.SuccessMessage] = $"Успешно изтрит кошер!";
            return RedirectToAction(nameof(this.AllByApiaryId), new { id = apiaryId });
        }

        public async Task<IActionResult> ExportToExcel(int? id)
        {
            var currentUser = await userManager.GetUserAsync(User);

            var result = excelExportService.ExportAsExcelBeehive(currentUser.Id, id);

            Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            return new FileContentResult(result.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<IActionResult> Bookmark(int id, string returnUrl)
        {
            var apiaryId = await beehiveService.BookmarkBeehiveAsync(id);

            if (!apiaryId.HasValue)
            {
                return BadRequest();
            }

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> SelectApiaryToMoveBeehive(int id)
        {
            var currentUser = await userManager.GetUserAsync(User);

            var inputModel = new SelectApiaryToMoveBeehiveIn
            {
                BeehiveId = id,
                BeehiveNumber = beehiveService.GetBeehiveNumberById(id),
                BeehiveApiaryId = apiaryService.GetApiaryIdByBeehiveId(id),
                BeehiveApiaryNumber = apiaryService.GetApiaryNumberByBeehiveId(id),
                BeehiveApiaryName = apiaryService.GetApiaryNameByBeehiveId(id),
                AllApiaries = apiaryService.GetUserApiariesWithoutTemporaryAsKeyValuePairs(currentUser.Id),
            };

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> SelectApiaryToMoveBeehive(int id, SelectApiaryToMoveBeehiveIn inputModel)
        {
            var currentUser = await userManager.GetUserAsync(User);

            //if (beehiveService.BeehiveExistsInApiary(id, inputModel.SelectedApiaryId))
            //{
            //    ModelState.AddModelError(string.Empty, "Кошерът вече се намира в избрания пчелин.");
            //}

            if (!ModelState.IsValid)
            {
                inputModel.AllApiaries = apiaryService.GetUserApiariesWithoutTemporaryAsKeyValuePairs(currentUser.Id);
                return View(inputModel);
            }

            if (beehiveService.BeehiveNumberExistsInApiary(inputModel.BeehiveNumber, inputModel.SelectedApiaryId))
            {
                return RedirectToAction(nameof(this.ChooseNewNumberForBeehive), new { id = id, selectedApiaryId = inputModel.SelectedApiaryId});
            }

            beehiveService.UpdateBeehiveApiary(id, inputModel.SelectedApiaryId);

            var messageText = $"Успешно преместихте кошер №{inputModel.BeehiveNumber}!";

            TempData[GlobalConstants.SuccessMessage] = messageText;
            return RedirectToAction(nameof(this.ById), new { id = id });
        }

        public async Task<IActionResult> ChooseNewNumberForBeehive(int id, int selectedApiaryId)
        {
            var currentUser = await userManager.GetUserAsync(User);

            var inputModel = new ChooseNewNumberForBeehiveInputModel
            {
                BeehiveId = id,
                BeehiveNumber = beehiveService.GetBeehiveNumberById(id),
                BeehiveApiaryId = selectedApiaryId,
                BeehiveApiaryName = apiaryService.GetApiaryNameByApiaryId(selectedApiaryId),
                BeehiveApiaryNumber = apiaryService.GetApiaryNumberByApiaryId(selectedApiaryId),
            };

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChooseNewNumberForBeehive(int id, ChooseNewNumberForBeehiveInputModel inputModel)
        {
            var currentUser = await userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            beehiveService.UpdateBeehiveNumber(id, inputModel.BeehiveNumber);
            beehiveService.UpdateBeehiveApiary(id, inputModel.BeehiveApiaryId);

            var messageText = $"Успешно преместихте кошер №{inputModel.BeehiveNumber}!";

            TempData[GlobalConstants.SuccessMessage] = messageText;
            return RedirectToAction(nameof(this.ById), new { id = id });
        }
    }
}
