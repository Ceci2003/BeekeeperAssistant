namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using BeekeeperAssistant.Web.ViewModels.Inspection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    public class InspectionController : BaseController
    {
        private readonly IInspectionService inspectionService;
        private readonly IBeehiveService beehiveService;
        private readonly IApiaryService apiaryService;
        private readonly IForecastService forecastService;
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;

        public InspectionController(
            IInspectionService inspectionService,
            IBeehiveService beehiveService,
            IApiaryService apiaryService,
            IForecastService forecastService,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            this.inspectionService = inspectionService;
            this.beehiveService = beehiveService;
            this.apiaryService = apiaryService;
            this.forecastService = forecastService;
            this.configuration = configuration;
            this.userManager = userManager;
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

                var apiary = this.apiaryService.GetUserApiaryByBeehiveId<ApiaryViewModel>(id.Value);
                ForecastResult forecastResult = await this.forecastService.GetCurrentWeather(apiary.Adress, this.configuration["OpenWeatherMap:ApiId"]);
                if (forecastResult != null)
                {
                    inputModel.IncludeWeatherInfo = true;
                    inputModel.Conditions = forecastResult.Description;
                    inputModel.WeatherTemperatureString = forecastResult.Temp;
                    inputModel.WeatherHumidityString = forecastResult.Humidity;
                }
            }

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, CreateInspectionInputModel inputModel)
        {
            var currentuser = await this.userManager.GetUserAsync(this.User);

            var apiary = this.apiaryService.GetApiaryById<ApiaryViewModel>(inputModel.ApiaryId);
            var beehive = this.beehiveService.GetBeehiveByNumber<BeehiveViewModel>(inputModel.SelectedBeehiveNumber, apiary.Number);

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
                if (inputModel.Conditions != null || inputModel.WeatherTemperature != null || inputModel.WeatherHumidity != null)
                {
                    inputModel.IncludeWeatherInfo = true;
                }
            }

            if (id == null)
            {
                await this.inspectionService.CreateUserInspectionAsync(currentuser.Id, beehive.Id, inputModel);

                return this.RedirectToAction("ById", "Beehive", new { beehiveId = beehive.Id, tabPage = "Inspections" });
            }
            else
            {
                await this.inspectionService.CreateUserInspectionAsync(currentuser.Id, id.Value, inputModel);

                return this.RedirectToAction("ById", "Beehive", new { beehiveId = id.Value, tabPage = "Inspections" });
            }
        }

        public IActionResult Edit(int id)
        {
            var inputModel = this.inspectionService.GetInspectionById<EditInspectionInputModel>(id);
            inputModel.WeatherHumidityString = inputModel.WeatherHumidity.ToString();
            inputModel.WeatherTemperatureString = inputModel.WeatherTemperature.ToString();
            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditInspectionInputModel inputModel)
        {
            inputModel.WeatherHumidity = Convert.ToDouble(inputModel.WeatherHumidityString);
            inputModel.WeatherTemperature = Convert.ToDouble(inputModel.WeatherTemperatureString);

            await this.inspectionService.EditUserInspectionAsync(id, inputModel);

            return this.RedirectToAction("ById", "Beehive", new { beehiveId = inputModel.BeehiveId, tabPage = "Inspections" });
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

            return this.RedirectToAction("ById", "Beehive", new { beehiveId = inspection.BeehiveId, tabPage = "Inspections" });
        }
    }
}
