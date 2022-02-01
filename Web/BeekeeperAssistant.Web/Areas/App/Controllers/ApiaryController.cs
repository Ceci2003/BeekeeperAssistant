namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.ApiaryHelpers;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    [Authorize]
    public class ApiaryController : AppBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;
        private readonly IApiaryNumberService apiaryNumberService;
        private readonly IConfiguration configuration;
        private readonly IForecastService forecastService;
        private readonly IApiaryHelperService apiaryHelperService;
        private readonly IBeehiveHelperService beehiveHelperService;
        private readonly IExcelExportService excelExportService;

        public ApiaryController(
            UserManager<ApplicationUser> userManager,
            IApiaryService apiaryService,
            IBeehiveService beehiveService,
            IApiaryNumberService apiaryNumberService,
            IConfiguration configuration,
            IForecastService forecastService,
            IApiaryHelperService apiaryHelperService,
            IBeehiveHelperService beehiveHelperService,
            IExcelExportService excelExportService)
        {
            this.userManager = userManager;
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
            this.apiaryNumberService = apiaryNumberService;
            this.configuration = configuration;
            this.forecastService = forecastService;
            this.apiaryHelperService = apiaryHelperService;
            this.beehiveHelperService = beehiveHelperService;
            this.excelExportService = excelExportService;
        }

        public async Task<IActionResult> All(int pageAllApiaries = 1, int pageHelperApiaries = 1)
        {
            if (pageAllApiaries <= 0)
            {
                pageAllApiaries = 1;
            }

            if (pageHelperApiaries <= 0)
            {
                pageHelperApiaries = 1;
            }

            var currentUser = await userManager.GetUserAsync(User);

            var userApiariesCount = apiaryService.GetAllUserApiariesCount(currentUser.Id);
            var pagesApiaryCount = (int)Math.Ceiling((double)userApiariesCount / GlobalConstants.ApiariesPerPage);

            var apiaryHelperCount = apiaryHelperService.GetUserHelperApiariesCount(currentUser.Id);
            var pagesApiaryHelperCount = (int)Math.Ceiling((double)apiaryHelperCount / GlobalConstants.ApiaryHelpersApiaryPerPage);

            var viewModel = new AllApiaryViewModel
            {
                UserApiaries = new AllApiaryUserApiariesViewModel
                {
                    AllUserApiaries = apiaryService.GetAllUserApiaries<AllApiaryUserApiariesDataViewModel>(
                        currentUser.Id,
                        GlobalConstants.ApiariesPerPage,
                        (pageAllApiaries - 1) * GlobalConstants.ApiariesPerPage),
                    PagesCount = pagesApiaryCount,
                },
                UserHelperApiaries = new AllApiaryUserHelperApiariesViewModel
                {
                    AllUserHelperApiaries = apiaryHelperService.GetUserHelperApiaries<AllApiaryUserHelperApiariesDataViewModel>(
                        currentUser.Id,
                        GlobalConstants.ApiaryHelpersApiaryPerPage,
                        (pageHelperApiaries - 1) * GlobalConstants.ApiaryHelpersApiaryPerPage),
                    PagesCount = pagesApiaryHelperCount,
                },
            };

            foreach (var apiary in viewModel.UserHelperApiaries.AllUserHelperApiaries)
            {
                apiary.Access = await apiaryHelperService.GetUserApiaryAccessAsync(currentUser.Id, apiary.ApiaryId);
            }

            if (viewModel.UserApiaries.PagesCount == 0)
            {
                viewModel.UserApiaries.PagesCount = 1;
            }

            if (viewModel.UserHelperApiaries.PagesCount == 0)
            {
                viewModel.UserHelperApiaries.PagesCount = 1;
            }

            viewModel.UserApiaries.CurrentPage = pageAllApiaries;
            viewModel.UserHelperApiaries.CurrentPage = pageHelperApiaries;

            return View(viewModel);
        }

        public async Task<IActionResult> AllMovable(int pageAllApiaries = 1)
        {
            if (pageAllApiaries <= 0)
            {
                pageAllApiaries = 1;
            }

            var currentUser = await userManager.GetUserAsync(User);

            var userApiariesCount = apiaryService.GetAllUserApiariesCount(currentUser.Id);
            var pagesApiaryCount = (int)Math.Ceiling((double)userApiariesCount / GlobalConstants.ApiariesPerPage);

            var viewModel = new AllApiaryUserMovableApiariesViewModel
            {
                AllUserMovableApiaries = apiaryService.GetAllUserMovableApiaries<AllApiaryUserMovableApiariesDataViewModel>(
                        currentUser.Id,
                        GlobalConstants.ApiariesPerPage,
                        (pageAllApiaries - 1) * GlobalConstants.ApiariesPerPage),
                PagesCount = pagesApiaryCount,
            };

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = pageAllApiaries;

            return View(viewModel);
        }

        public async Task<IActionResult> ById(int id)
        {
            var viewModel = apiaryService.GetApiaryById<ByNumberApiaryViewModel>(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            var currentUser = await userManager.GetUserAsync(User);

            if (viewModel.CreatorId != currentUser.Id &&
                !apiaryHelperService.IsApiaryHelper(currentUser.Id, viewModel.Id) &&
                !await userManager.IsInRoleAsync(currentUser, GlobalConstants.AdministratorRoleName))
            {
                return BadRequest();
            }

            viewModel.ApiaryAccess = await apiaryHelperService.GetUserApiaryAccessAsync(currentUser.Id, viewModel.Id);

            if (viewModel.Number != null)
            {
                var postcode = viewModel.Number.Split('-')[0];
                viewModel.ForecastResult =
                    await forecastService.GetApiaryCurrentWeatherByCityPostcode(postcode, configuration["OpenWeatherMap:ApiId"]);

                if (viewModel.ForecastResult == null)
                {
                    viewModel.ForecastResult =
                        await forecastService.GetApiaryCurrentWeatherByCityName(viewModel.Adress, configuration["OpenWeatherMap:ApiId"]);
                }
            }
            else
            {
                viewModel.ForecastResult =
                    await forecastService.GetApiaryCurrentWeatherByCityName(viewModel.Adress, configuration["OpenWeatherMap:ApiId"]);
            }

            return View(viewModel);
        }

        public IActionResult Create()
        {
            var inputModel = new CreateApiaryInputModel();
            inputModel.IsRegistered = true;

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateApiaryInputModel inputModel, string returnUrl)
        {
            if (inputModel.IsRegistered && inputModel.CityCode != null)
            {
                var postcode = inputModel.CityCode.Split('-')[0];
                if (!forecastService.ValidateCityPostcode(postcode, configuration["OpenWeatherMap:ApiId"]))
                {
                    ModelState.AddModelError(string.Empty, "Не съществува населено място с въведения пощенски код.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            var currentUser = await userManager.GetUserAsync(User);

            var apiaryId =
                await apiaryService.CreateUserApiaryAsync(
                    currentUser.Id,
                    inputModel.Number,
                    inputModel.Name,
                    inputModel.ApiaryType,
                    inputModel.Adress,
                    inputModel.IsRegistered,
                    inputModel.IsClosed,
                    inputModel.OpeningDate,
                    inputModel.ClosingDate);

            TempData[GlobalConstants.SuccessMessage] = $"Успешно създаден пчелин!";

            return RedirectToAction(nameof(this.ById), new { id = apiaryId });
        }

        public IActionResult Edit(int id)
        {
            var viewModel = apiaryService.GetApiaryById<EditApiaryInputModel>(id);

            if (viewModel.IsRegistered)
            {
                viewModel.CityCode = apiaryNumberService.GetCityCode(viewModel.Number);
                viewModel.FarmNumber = apiaryNumberService.GetFarmNumber(viewModel.Number);
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditApiaryInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            var modelNumber = apiaryNumberService.CreateApiaryNumber(inputModel.CityCode, inputModel.FarmNumber);
            if (!inputModel.IsRegistered)
            {
                modelNumber = null;
            }

            var apiaryId =
                await apiaryService.EditApiaryByIdAsync(
                    id,
                    modelNumber,
                    inputModel.Name,
                    inputModel.ApiaryType,
                    inputModel.Adress,
                    inputModel.IsRegistered,
                    inputModel.IsClosed,
                    inputModel.OpeningDate,
                    inputModel.ClosingDate);

            TempData[GlobalConstants.SuccessMessage] = $"Успешно редактиран пчелин!";
            return RedirectToAction(nameof(this.ById), new { id = apiaryId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, string returnUrl)
        {
            await apiaryService.DeleteApiaryByIdAsync(id);

            TempData[GlobalConstants.SuccessMessage] = $"Успешно изтрит пчелин!";

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> SelectApiaryToAddBeehiveInTemporary(int id)
        {
            var currentUser = await userManager.GetUserAsync(User);

            var inputModel = new SelectApiaryToAddBeehiveInTemporaryInputModel
            {
                TemporaryId = id,
                TemporaryNumber = apiaryService.GetApiaryNumberByApiaryId(id),
                TemporaryName = apiaryService.GetApiaryNameByApiaryId(id),
                AllApiaries = apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id),
            };

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> SelectApiaryToAddBeehiveInTemporary(int id, SelectApiaryToAddBeehiveInTemporaryInputModel inputModel)
        {
            return RedirectToAction(nameof(this.SelectBeehivesToAddInTemporary), new { id = inputModel.SelectedApiaryId, temporaryId = id });
        }

        public async Task<IActionResult> SelectBeehivesToAddInTemporary(int apiaryid, int temporaryId)
        {
            return View();
        }

        public async Task<IActionResult> ExportToExcel()
        {
            var currentUser = await userManager.GetUserAsync(User);

            var exportResult = excelExportService.ExportAsExcelApiary(currentUser.Id);

            Response.Headers.Add("content-disposition", "attachment: filename=ExcelReport.xlsx");
            return new FileContentResult(exportResult.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<IActionResult> Bookmark(int id)
        {
            await apiaryService.BookmarkApiaryAsync(id);
            return RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> UpdateMovableStatus(int id)
        {
            await this.apiaryService.UpdateMovableStatus(id);
            return RedirectToAction(nameof(this.ById), new { id = id });
        }
    }
}
