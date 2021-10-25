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
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;

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

        public ApiaryController(
            UserManager<ApplicationUser> userManager,
            IApiaryService apiaryService,
            IBeehiveService beehiveService,
            IApiaryNumberService apiaryNumberService,
            IConfiguration configuration,
            IForecastService forecastService,
            IApiaryHelperService apiaryHelperService)
        {
            this.userManager = userManager;
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
            this.apiaryNumberService = apiaryNumberService;
            this.configuration = configuration;
            this.forecastService = forecastService;
            this.apiaryHelperService = apiaryHelperService;
        }

        public async Task<IActionResult> All(int pageAllApiaries = 1, int pageHelperApiaries = 1)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var apiariesUserCount = this.apiaryService.GetAllUserApiariesCount(currentUser.Id);
            var pagesApiaryCount = (int)Math.Ceiling((double)apiariesUserCount / GlobalConstants.ApiariesPerPage);

            var apiaryHelperCount = this.apiaryHelperService.GetUserHelperApiariesCount(currentUser.Id);
            var pagesApiaryHelperCount = (int)Math.Ceiling((double)apiaryHelperCount / GlobalConstants.ApiaryHelpersApiaryPerPage);

            if (pageAllApiaries <= 0)
            {
                pageAllApiaries = 1;
            }
            else if (pageAllApiaries > pagesApiaryCount)
            {
                pageAllApiaries = pagesApiaryCount == 0 ? 1 : pagesApiaryCount;
            }

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
                    AllUserApiaries = this.apiaryService.GetAllUserApiaries<ApiaryViewModel>(currentUser.Id, GlobalConstants.ApiariesPerPage, (pageAllApiaries - 1) * GlobalConstants.ApiariesPerPage),
                    PagesCount = pagesApiaryCount,
                },
                UserHelperApiaries = new AllHelperApiariesViewModel
                {
                    AllUserHelperApiaries = this.apiaryHelperService.GetUserHelperApiaries<ApiaryViewModel>(currentUser.Id, GlobalConstants.ApiaryHelpersApiaryPerPage, (pageHelperApiaries - 1) * GlobalConstants.ApiaryHelpersApiaryPerPage),
                    PagesCount = pagesApiaryHelperCount,
                },
            };

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

        public async Task<IActionResult> ByNumber(string apiaryNumber, int pageOne = 1)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var viewModel = this.apiaryService.GetApiaryByNumber<ApiaryDataViewModel>(apiaryNumber);

            if (viewModel.CreatorId != currentUser.Id &&
                !this.apiaryHelperService.IsApiaryHelper(currentUser.Id, viewModel.Id))
            {
                return this.BadRequest();
            }

            ForecastResult forecastResult = await this.forecastService.GetCurrentWeather(viewModel.Adress, this.configuration["OpenWeatherMap:ApiId"]);
            viewModel.ForecastResult = forecastResult;

            viewModel.Beehives = this.beehiveService.GetBeehivesByApiaryId<BeehiveViewModel>(viewModel.Id, GlobalConstants.BeehivesPerPage, (pageOne - 1) * GlobalConstants.BeehivesPerPage);
            var count = this.beehiveService.GetAllBeehivesCountByApiaryId(viewModel.Id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / GlobalConstants.BeehivesPerPage);

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = pageOne;

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateApiaryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var apiaryNumber = await this.apiaryService.CreateUserApiaryAsync(currentUser.Id, inputModel.Number, inputModel.Name, inputModel.ApiaryType, inputModel.Adress);

            return this.Redirect($"/Apiary/{apiaryNumber}");
        }

        public IActionResult Edit(int id)
        {
            var viewModel = this.apiaryService.GetApiaryById<EditApiaryInputModel>(id);

            viewModel.CityCode = this.apiaryNumberService.GetCityCode(viewModel.Number);
            viewModel.FarmNumber = this.apiaryNumberService.GetFarmNumber(viewModel.Number);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditApiaryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            inputModel.Number = this.apiaryNumberService.CreateApiaryNumber(inputModel.CityCode, inputModel.FarmNumber);

            var apiaryNumber = await this.apiaryService.EditApiaryByIdAsync(id, inputModel.Number, inputModel.Name, inputModel.ApiaryType, inputModel.Adress);

            return this.Redirect($"/Apiary/{apiaryNumber}");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await this.apiaryService.DeleteApiaryByIdAsync(id);
            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> ExportToExcel()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var apiaries = this.apiaryService.GetAllUserApiaries<ApiaryDataViewModel>(currentUser.Id);

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1:B1"].Merge = true;
            ws.Cells["A1"].Value = "Доклад - Кошери";
            ws.Cells["A2:B2"].Merge = true;
            ws.Cells["A2"].Value = $"Дата: {string.Format("{0:dd-MM-yyyy} {0:H:mm}", DateTimeOffset.Now)}";

            ws.Cells["A4"].Value = "Номер";
            ws.Cells["B4"].Value = "Адрес";
            ws.Cells["C4"].Value = "Име";
            ws.Cells["D4"].Value = "Вид";

            // ws.Cells["E4"].Value = "Брой кошери";
            ws.Cells["A4:E4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A4:E4"].Style.Fill.BackgroundColor.SetColor(1, 183, 225, 205);
            ws.Cells["A4:E4"].Style.Font.Color.SetColor(Color.White);

            int rowIndex = 5;
            foreach (var apiary in apiaries)
            {
                ws.Cells[$"A{rowIndex}"].Value = apiary.Number;
                ws.Cells[$"B{rowIndex}"].Value = apiary.Adress;
                ws.Cells[$"C{rowIndex}"].Value = apiary.Name == null ? "-" : apiary.Name;
                ws.Cells[$"D{rowIndex}"].Value = apiary.ApiaryType;

                // ws.Cells[$"D{rowIndex}"].Value = apiary.Beehives.ToList().Count();
                rowIndex++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();

            this.Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            return new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<IActionResult> Bookmark(int id)
        {
            await this.apiaryService.BookmarkApiaryAsync(id);
            return this.Redirect("/Apiary/All");
        }
    }
}
