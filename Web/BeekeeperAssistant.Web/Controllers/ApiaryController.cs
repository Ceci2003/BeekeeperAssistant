namespace BeekeeperAssistant.Web.Controllers
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
    public class ApiaryController : BaseController
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
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var userApiariesCount = this.apiaryService.GetAllUserApiariesCount(currentUser.Id);
            var pagesApiaryCount = (int)Math.Ceiling((double)userApiariesCount / GlobalConstants.ApiariesPerPage);

            if (pageAllApiaries <= 0)
            {
                pageAllApiaries = 1;
            }
            else if (pageAllApiaries > pagesApiaryCount)
            {
                pageAllApiaries = pagesApiaryCount == 0 ? 1 : pagesApiaryCount;
            }

            var apiaryHelperCount = this.apiaryHelperService.GetUserHelperApiariesCount(currentUser.Id);
            var pagesApiaryHelperCount = (int)Math.Ceiling((double)apiaryHelperCount / GlobalConstants.ApiaryHelpersApiaryPerPage);

            if (pageHelperApiaries <= 0)
            {
                pageHelperApiaries = 1;
            }
            else if (pageHelperApiaries > pagesApiaryHelperCount)
            {
                pageHelperApiaries = pagesApiaryHelperCount == 0 ? 1 : pagesApiaryHelperCount;
            }

            var viewModel = new AllApiariesViewModel
            {
                UserApiaries = new AllUserApiariesViewModel
                {
                    AllUserApiaries = this.apiaryService.GetAllUserApiaries<ApiaryViewModel>(
                        currentUser.Id,
                        GlobalConstants.ApiariesPerPage,
                        (pageAllApiaries - 1) * GlobalConstants.ApiariesPerPage),
                    PagesCount = pagesApiaryCount,
                },
                UserHelperApiaries = new AllHelperApiariesViewModel
                {
                    AllUserHelperApiaries = this.apiaryHelperService.GetUserHelperApiaries<ApiaryHelperApiaryDataViewModel>(
                        currentUser.Id,
                        GlobalConstants.ApiaryHelpersApiaryPerPage,
                        (pageHelperApiaries - 1) * GlobalConstants.ApiaryHelpersApiaryPerPage),
                    PagesCount = pagesApiaryHelperCount,
                },
            };

            foreach (var apiary in viewModel.UserHelperApiaries.AllUserHelperApiaries)
            {
                apiary.Access = await this.apiaryHelperService.GetUserApiaryAccessAsync(currentUser.Id, apiary.ApiaryId);
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

            return this.View(viewModel);
        }

        public async Task<IActionResult> ByNumber(string apiaryNumber)
        {
            var viewModel = this.apiaryService.GetApiaryByNumber<ApiaryDataViewModel>(apiaryNumber);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (viewModel.CreatorId != currentUser.Id &&
                !this.apiaryHelperService.IsApiaryHelper(currentUser.Id, viewModel.Id) &&
                !await this.userManager.IsInRoleAsync(currentUser, GlobalConstants.AdministratorRoleName))
            {
                return this.BadRequest();
            }

            viewModel.ApiaryAccess = await this.apiaryHelperService.GetUserApiaryAccessAsync(currentUser.Id, viewModel.Id);

            viewModel.ForecastResult =
                await this.forecastService.GetCurrentWeather(viewModel.Adress, this.configuration["OpenWeatherMap:ApiId"]);

            return this.View(viewModel);
        }

        // DONE []
        public IActionResult Create()
        {
            return this.View();
        }

        // DONE []
        [HttpPost]
        public async Task<IActionResult> Create(CreateApiaryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            var apiaryNumber =
                await this.apiaryService.CreateUserApiaryAsync(
                    currentUser.Id,
                    inputModel.Number,
                    inputModel.Name,
                    inputModel.ApiaryType,
                    inputModel.Adress);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно създаден пчелин!";

            return this.Redirect($"/Apiary/{apiaryNumber}");
        }

        // DONE []
        public IActionResult Edit(int id)
        {
            var viewModel = this.apiaryService.GetApiaryById<EditApiaryInputModel>(id);

            viewModel.CityCode = this.apiaryNumberService.GetCityCode(viewModel.Number);
            viewModel.FarmNumber = this.apiaryNumberService.GetFarmNumber(viewModel.Number);

            return this.View(viewModel);
        }

        // DONE []
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditApiaryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            inputModel.Number = this.apiaryNumberService.CreateApiaryNumber(inputModel.CityCode, inputModel.FarmNumber);

            var apiaryNumber =
                await this.apiaryService.EditApiaryByIdAsync(
                    id,
                    inputModel.Number,
                    inputModel.Name,
                    inputModel.ApiaryType,
                    inputModel.Adress);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно редактиран пчелин!";
            return this.Redirect($"/Apiary/{apiaryNumber}");
        }

        // DONE []
        [HttpPost]
        public async Task<IActionResult> Delete(int id, string returnUrl)
        {
            await this.apiaryService.DeleteApiaryByIdAsync(id);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно изтрит пчелин!";

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction(nameof(this.All));
        }

        // DONE []
        public async Task<IActionResult> ExportToExcel()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var exportResult = this.excelExportService.ExportAsExcelApiary(currentUser.Id);

            this.Response.Headers.Add("content-disposition", "attachment: filename=ExcelReport.xlsx");
            return new FileContentResult(exportResult.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        // DONE []
        public async Task<IActionResult> Bookmark(int id)
        {
            await this.apiaryService.BookmarkApiaryAsync(id);
            return this.Redirect("/Apiary/All");
        }
    }
}
