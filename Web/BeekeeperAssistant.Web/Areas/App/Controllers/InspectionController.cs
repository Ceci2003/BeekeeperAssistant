namespace BeekeeperAssistant.Web.Areas.App.Controllers
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

    public class InspectionController : AppBaseController
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

            var currentUser = await userManager.GetUserAsync(User);

            var viewModel = new AllByBeehiveIdInspectionViewModel()
            {
                AllInspections =
                    inspectionService.GetAllBeehiveInspections<AllByBeehiveIdInspectionAllInspectionsViewModel>(id, GlobalConstants.ApiariesPerPage, (page - 1) * GlobalConstants.ApiariesPerPage),
            };

            viewModel.BeehiveId = id;
            viewModel.BeehiveNumber = beehiveService.GetBeehiveNumberById(id);
            viewModel.BeehiveAccess = await beehiveHelperService.GetUserBeehiveAccessAsync(currentUser.Id, id);

            var apiary = apiaryService.GetUserApiaryByBeehiveId<ApiaryDataModel>(id);
            viewModel.ApiaryId = apiary.Id;
            viewModel.ApiaryNumber = apiary.Number;
            viewModel.ApiaryName = apiary.Name;

            var count = inspectionService.GetAllBeehiveInspectionsCountByBeehiveId(id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / GlobalConstants.ApiariesPerPage);

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = page;

            return View(viewModel);
        }

        public async Task<IActionResult> Create(int? id)
        {
            var currentuser = await userManager.GetUserAsync(User);

            var inputModel = new CreateInspectionInputModel
            {
                DateOfInspection = DateTime.UtcNow.Date,
            };

            if (id == null)
            {
                inputModel.Apiaries = apiaryService.GetUserApiariesAsKeyValuePairs(currentuser.Id);
            }
            else
            {
                inputModel.ApiaryId = apiaryService.GetApiaryIdByBeehiveId(id.Value);

                var apiary = apiaryService.GetUserApiaryByBeehiveId<ApiaryDataModel>(id.Value);
                var beehive = beehiveService.GetBeehiveById<BeehiveDataModel>(id.Value);

                ForecastResult forecastResult = await forecastService.GetApiaryCurrentWeatherByCityName(apiary.Adress, configuration["OpenWeatherMap:ApiId"]);
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

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, CreateInspectionInputModel inputModel)
        {
            var currentuser = await userManager.GetUserAsync(User);

            var apiaryNumber = apiaryService.GetApiaryNumberByApiaryId(inputModel.ApiaryId);
            var beehive = beehiveService.GetBeehiveByNumber<BeehiveDataModel>(inputModel.SelectedBeehiveNumber, apiaryNumber);

            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            if (id == null && beehive == null)
            {
                ModelState.AddModelError(string.Empty, $"Не съществува кошер с номер {inputModel.SelectedBeehiveNumber} в пчелина!");
            }

            if (!ModelState.IsValid)
            {
                if (id == null)
                {
                    inputModel.Apiaries = apiaryService.GetUserApiariesAsKeyValuePairs(currentuser.Id);
                }
                else
                {
                    inputModel.ApiaryId = apiaryService.GetApiaryIdByBeehiveId(id.Value);
                }

                return View(inputModel);
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

            var apiaryOwnerId = apiaryService.GetApiaryOwnerIdByApiaryId(inputModel.ApiaryId);

            if (id == null)
            {
                await inspectionService.CreateUserInspectionAsync(apiaryOwnerId, currentuser.Id, beehive.Id, inputModel);
            }
            else
            {
                await inspectionService.CreateUserInspectionAsync(apiaryOwnerId, currentuser.Id, id.Value, inputModel);
            }

            TempData[GlobalConstants.SuccessMessage] = $"Успешно създаден преглед!";
            return RedirectToAction(nameof(this.AllByBeehiveId), new { id = id.Value });
        }

        public IActionResult Edit(int id, int beehiveId)
        {
            var inputModel = inspectionService.GetInspectionById<EditInspectionInputModel>(id);
            inputModel.WeatherHumidityString = inputModel.WeatherHumidity.ToString();
            inputModel.WeatherTemperatureString = inputModel.WeatherTemperature.ToString();

            var apiaryNumber = apiaryService.GetApiaryNumberByBeehiveId(beehiveId);
            var beehiveNumber = beehiveService.GetBeehiveNumberById(beehiveId);
            inputModel.BeehiveId = beehiveId;
            inputModel.ApiaryNumber = apiaryNumber;
            inputModel.BeehiveNumber = beehiveNumber;

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditInspectionInputModel inputModel)
        {
            inputModel.WeatherHumidity = Convert.ToDouble(inputModel.WeatherHumidityString);
            inputModel.WeatherTemperature = Convert.ToDouble(inputModel.WeatherTemperatureString);

            var beehiveId = await inspectionService.EditUserInspectionAsync(id, inputModel);

            TempData[GlobalConstants.SuccessMessage] = $"Успешно редактиран преглед!";
            return RedirectToAction(nameof(this.AllByBeehiveId), new { id = beehiveId });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = await userManager.GetUserAsync(User);
            var inspection = inspectionService.GetInspectionById<InspectionDataViewModel>(id);

            if (inspection.CreatorId != currentUser.Id)
            {
                return BadRequest();
            }

            await inspectionService.DeleteInspectionAsync(id);

            TempData[GlobalConstants.SuccessMessage] = $"Успешно изтрит преглед!";
            return RedirectToAction(nameof(this.AllByBeehiveId), new { id = inspection.BeehiveId });
        }

        public IActionResult ExportToExcel(int id)
        {
            var pck = excelExportService.ExportAsExcelInspection(id);

            Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            return new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
