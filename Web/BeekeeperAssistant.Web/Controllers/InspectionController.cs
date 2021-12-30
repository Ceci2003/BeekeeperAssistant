namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using BeekeeperAssistant.Web.ViewModels.Inspections;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;

    public class InspectionController : BaseController
    {
        private readonly IInspectionService inspectionService;
        private readonly IBeehiveService beehiveService;
        private readonly IApiaryService apiaryService;
        private readonly IForecastService forecastService;
        private readonly IConfiguration configuration;
        private readonly IExcelExportService excelExportService;
        private readonly IBeehiveHelperService beehiveHelperService;
        private readonly UserManager<ApplicationUser> userManager;

        public InspectionController(
            IInspectionService inspectionService,
            IBeehiveService beehiveService,
            IApiaryService apiaryService,
            IForecastService forecastService,
            IConfiguration configuration,
            IExcelExportService excelExportService,
            IBeehiveHelperService beehiveHelperService,
            UserManager<ApplicationUser> userManager)
        {
            this.inspectionService = inspectionService;
            this.beehiveService = beehiveService;
            this.apiaryService = apiaryService;
            this.forecastService = forecastService;
            this.configuration = configuration;
            this.excelExportService = excelExportService;
            this.beehiveHelperService = beehiveHelperService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> AllByBeehiveId(int id, int page = 1)
        {
            if (page <= 0)
            {
                page = 1;
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            var viewModel = new AllByBeehiveIdInspectionViewModel()
            {
                AllInspections =
                    this.inspectionService.GetAllBeehiveInspections<AllByBeehiveIdInspectionAllInspectionsViewModel>(id, GlobalConstants.ApiariesPerPage, (page - 1) * GlobalConstants.ApiariesPerPage),
            };

            viewModel.BeehiveId = id;
            viewModel.BeehiveNumber = this.beehiveService.GetBeehiveNumberById(id);
            viewModel.BeehiveAccess = await this.beehiveHelperService.GetUserBeehiveAccessAsync(currentUser.Id, id);

            var apiary = this.apiaryService.GetUserApiaryByBeehiveId<ApiaryDataModel>(id);
            viewModel.ApiaryId = apiary.Id;
            viewModel.ApiaryNumber = apiary.Number;
            viewModel.ApiaryName = apiary.Name;

            var count = this.inspectionService.GetAllBeehiveInspectionsCountByBeehiveId(id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / GlobalConstants.ApiariesPerPage);

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        public async Task<IActionResult> Create(int? id)
        {
            var currentuser = await this.userManager.GetUserAsync(this.User);

            var inputModel = new CreateInspectionInputModel
            {
                DateOfInspection = DateTime.UtcNow.Date,
            };

            if (id == null)
            {
                inputModel.Apiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentuser.Id);
            }
            else
            {
                inputModel.ApiaryId = this.apiaryService.GetApiaryIdByBeehiveId(id.Value);

                var apiary = this.apiaryService.GetUserApiaryByBeehiveId<ApiaryDataModel>(id.Value);
                var beehive = this.beehiveService.GetBeehiveById<BeehiveDataModel>(id.Value);

                ForecastResult forecastResult = await this.forecastService.GetCurrentWeather(apiary.Adress, this.configuration["OpenWeatherMap:ApiId"]);
                if (forecastResult != null)
                {
                    inputModel.IncludeWeatherInfo = true;
                    inputModel.Conditions = forecastResult.Description;
                    inputModel.WeatherTemperatureString = forecastResult.Temp;
                    inputModel.WeatherHumidityString = forecastResult.Humidity;
                }

                inputModel.BeehiveId = id.Value;
                inputModel.ApiaryNumber = apiary.Number;
                inputModel.BeehiveNumber = beehive.Number;
            }

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, CreateInspectionInputModel inputModel)
        {
            var currentuser = await this.userManager.GetUserAsync(this.User);

            var apiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(inputModel.ApiaryId);
            var beehive = this.beehiveService.GetBeehiveByNumber<BeehiveDataModel>(inputModel.SelectedBeehiveNumber, apiaryNumber);

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            if (id == null && beehive == null)
            {
                this.ModelState.AddModelError(string.Empty, $"Не съществува кошер с номер {inputModel.SelectedBeehiveNumber} в пчелина!");
            }

            if (!this.ModelState.IsValid)
            {
                if (id == null)
                {
                    inputModel.Apiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentuser.Id);
                }
                else
                {
                    inputModel.ApiaryId = this.apiaryService.GetApiaryIdByBeehiveId(id.Value);
                }

                return this.View(inputModel);
            }

            if (!inputModel.IncludeQueenSection)
            {
                if (inputModel.QueenSeen || inputModel.QueenCells != QueenCells.None)
                {
                    inputModel.IncludeQueenSection = true;
                }
            }

            if (!inputModel.IncludeBrood)
            {
                if (inputModel.Eggs || inputModel.ClappedBrood || inputModel.UnclappedBrood)
                {
                    inputModel.IncludeBrood = true;
                }
            }

            if (!inputModel.IncludeFramesWith)
            {
                if (inputModel.FramesWithBees != 0 || inputModel.FramesWithBrood != 0 || inputModel.FramesWithHoney != 0 || inputModel.FramesWithPollen != 0)
                {
                    inputModel.IncludeFramesWith = true;
                }
            }

            if (!inputModel.IncludeActivity)
            {
                if (inputModel.BeeActivity != Activity.Low || inputModel.OrientationActivity != Activity.Low || inputModel.PollenActivity != Activity.Low || inputModel.ForragingActivity != Activity.Low || inputModel.BeesPerMinute != 0)
                {
                    inputModel.IncludeActivity = true;
                }
            }

            if (!inputModel.IncludeSpottedProblem)
            {
                if (inputModel.Disease != null || inputModel.Treatment != null || inputModel.Pests != null || inputModel.Predators != null)
                {
                    inputModel.IncludeSpottedProblem = true;
                }
            }

            if (!inputModel.IncludeWeatherInfo)
            {
                if (inputModel.Conditions != null)
                {
                    inputModel.IncludeWeatherInfo = true;
                }
            }

            var apiaryOwnerId = this.apiaryService.GetApiaryOwnerIdByApiaryId(inputModel.ApiaryId);

            if (id == null)
            {
                await this.inspectionService.CreateUserInspectionAsync(apiaryOwnerId, currentuser.Id, beehive.Id, inputModel);
            }
            else
            {
                await this.inspectionService.CreateUserInspectionAsync(apiaryOwnerId, currentuser.Id, id.Value, inputModel);
            }

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно създаден преглед!";
            return this.RedirectToAction("AllByBeehiveId", "Inspection", new { id = id.Value });
        }

        public IActionResult Edit(int id, int beehiveId)
        {
            var inputModel = this.inspectionService.GetInspectionById<EditInspectionInputModel>(id);
            inputModel.WeatherHumidityString = inputModel.WeatherHumidity.ToString();
            inputModel.WeatherTemperatureString = inputModel.WeatherTemperature.ToString();

            var apiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(beehiveId);
            var beehiveNumber = this.beehiveService.GetBeehiveNumberById(beehiveId);
            inputModel.BeehiveId = beehiveId;
            inputModel.ApiaryNumber = apiaryNumber;
            inputModel.BeehiveNumber = beehiveNumber;

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditInspectionInputModel inputModel)
        {
            inputModel.WeatherHumidity = Convert.ToDouble(inputModel.WeatherHumidityString);
            inputModel.WeatherTemperature = Convert.ToDouble(inputModel.WeatherTemperatureString);

            var beehiveId = await this.inspectionService.EditUserInspectionAsync(id, inputModel);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно редактиран преглед!";
            return this.RedirectToAction("AllByBeehiveId", "Inspection", new { id = beehiveId });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var inspection = this.inspectionService.GetInspectionById<InspectionDataViewModel>(id);

            if (inspection.CreatorId != currentUser.Id)
            {
                return this.BadRequest();
            }

            await this.inspectionService.DeleteInspectionAsync(id);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно изтрит преглед!";
            return this.RedirectToAction("AllByBeehiveId", "Inspection", new { id = inspection.BeehiveId });
        }

        public IActionResult ExportToExcel(int id)
        {
            var pck = this.excelExportService.ExportAsExcelInspection(id);

            this.Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            return new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
