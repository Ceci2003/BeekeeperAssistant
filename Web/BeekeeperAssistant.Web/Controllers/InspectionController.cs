namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
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
    using OfficeOpenXml;
    using OfficeOpenXml.Style;

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
                var beehive = this.beehiveService.GetBeehiveById<BeehiveViewModel>(id.Value);

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

        public IActionResult Edit(int id, int beehiveId)
        {
            var inputModel = this.inspectionService.GetInspectionById<EditInspectionInputModel>(id);
            inputModel.WeatherHumidityString = inputModel.WeatherHumidity.ToString();
            inputModel.WeatherTemperatureString = inputModel.WeatherTemperature.ToString();

            var apiary = this.apiaryService.GetUserApiaryByBeehiveId<ApiaryViewModel>(beehiveId);
            var beehive = this.beehiveService.GetBeehiveById<BeehiveViewModel>(beehiveId);
            inputModel.BeehiveId = beehiveId;
            inputModel.ApiaryNumber = apiary.Number;
            inputModel.BeehiveNumber = beehive.Number;

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

        public async Task<IActionResult> ExportToExcel(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var beehive = this.beehiveService.GetBeehiveById<BeehiveViewModel>(id);
            var inspection = this.inspectionService.GetAllBeehiveInspections<InspectionDataViewModel>(id).FirstOrDefault();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = $"Дата на прегледа:";
            ws.Cells["B1"].Value = DateTime.Now.ToString("dd-MM-yyyy");
            ws.Cells["A2"].Value = "Вид на прегледа:";
            ws.Cells["B2"].Value = inspection.InspectionType;
            ws.Cells["A3"].Value = "Роил ли се е:";
            ws.Cells["B3"].Value = inspection.Swarmed;
            ws.Cells["A4"].Value = "Сила на кошера:";
            ws.Cells["B4"].Value = inspection.BeehivePower;

            ws.Cells["D2"].Value = $"Пчелин №:";
            ws.Cells["E2"].Value = $"{beehive.ApiaryNumber}";
            ws.Cells["D3"].Value = $"Кошер №:";
            ws.Cells["E3"].Value = $"{beehive.Number}";

            ws.Cells["A4:B4"].Merge = true;
            ws.Cells["A4"].Value = $"Дата: {string.Format("{0:dd-MM-yyyy} {0:H:mm}", DateTimeOffset.Now)}";

            ws.Cells["A6:B6"].Merge = true;
            ws.Cells["A6:B6"].Style.Font.Bold = true;
            ws.Cells["A6:B6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A6"].Value = "Основна информация:";
            ws.Cells["A7"].Value = "Предприети действия:";
            ws.Cells["B7"].Value = inspection.BeeActivity;
            ws.Cells["A8"].Value = "Маса на кошера(кг.):";
            ws.Cells["B8"].Value = inspection.Weight;
            ws.Cells["A9"].Value = "Температура на кошера(t°):";
            ws.Cells["B9"].Value = inspection.HiveTemperature;
            ws.Cells["A10"].Value = "Влажност на кошера(%):";
            ws.Cells["B10"].Value = inspection.HiveHumidity;

            if (!inspection.IncludeQueenSection)
            {
                for (int i = 12; i < 17; i++)
                {
                    ws.Row(i).Hidden = true;
                }
            }

            ws.Cells["A12:B12"].Merge = true;
            ws.Cells["A12:B12"].Style.Font.Bold = true;
            ws.Cells["A12:B12"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A12"].Value = "Секция майка";
            ws.Cells["A13"].Value = "Забелязана майка:";
            ws.Cells["B13"].Value = inspection.QueenSeen;
            ws.Cells["A14"].Value = "Маточници:";
            ws.Cells["B14"].Value = inspection.QueenCells;
            ws.Cells["A15"].Value = "Работен статус на майката:";
            ws.Cells["B15"].Value = inspection.QueenWorkingStatus;
            ws.Cells["A16"].Value = "Сила на майката:";
            ws.Cells["B16"].Value = inspection.QueenPowerStatus;

            if (!inspection.IncludeBrood)
            {
                for (int i = 18; i < 22; i++)
                {
                    ws.Row(i).Hidden = true;
                }
            }

            ws.Cells["A18:B18"].Merge = true;
            ws.Cells["A18:B18"].Style.Font.Bold = true;
            ws.Cells["A18:B18"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A18"].Value = "Секция пило";
            ws.Cells["A19"].Value = "Яйца:";
            ws.Cells["B19"].Value = inspection.Eggs;
            ws.Cells["A20"].Value = "Запечатано:";
            ws.Cells["B20"].Value = inspection.ClappedBrood;
            ws.Cells["A21"].Value = "Излюпващо се:";
            ws.Cells["B21"].Value = inspection.UnclappedBrood;

            if (!inspection.IncludeFramesWith)
            {
                for (int i = 23; i < 28; i++)
                {
                    ws.Row(i).Hidden = true;
                }
            }

            ws.Cells["A23:B23"].Merge = true;
            ws.Cells["A23:B23"].Style.Font.Bold = true;
            ws.Cells["A23:B23"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A23"].Value = "Секция пити";
            ws.Cells["A24"].Value = "С пчели:";
            ws.Cells["B24"].Value = inspection.FramesWithBees;
            ws.Cells["A25"].Value = "С пило:";
            ws.Cells["B25"].Value = inspection.FramesWithBrood;
            ws.Cells["A26"].Value = "С мед:";
            ws.Cells["B26"].Value = inspection.FramesWithHoney;
            ws.Cells["A27"].Value = "С Прашец:";
            ws.Cells["B27"].Value = inspection.FramesWithPollen;

            if (!inspection.IncludeActivity)
            {
                for (int i = 29; i < 35; i++)
                {
                    ws.Row(i).Hidden = true;
                }
            }

            ws.Cells["A29:B29"].Merge = true;
            ws.Cells["A29:B29"].Style.Font.Bold = true;
            ws.Cells["A29:B29"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A29"].Value = "Секция активност";
            ws.Cells["A30"].Value = "Активност на пчелите:"; ws.Cells["B30"].Value = inspection.BeeActivity;
            ws.Cells["A31"].Value = "Ориентационни полети:"; ws.Cells["B31"].Value = inspection.OrientationActivity;
            ws.Cells["A32"].Value = "Принос на прашец:"; ws.Cells["B32"].Value = inspection.PollenActivity;
            ws.Cells["A33"].Value = "Принос на мед:"; ws.Cells["B33"].Value = inspection.ForragingActivity;
            ws.Cells["A34"].Value = "Пчели в минута:"; ws.Cells["B34"].Value = inspection.BeesPerMinute;

            if (!inspection.IncludeStorage)
            {
                for (int i = 36; i < 39; i++)
                {
                    ws.Row(i).Hidden = true;
                }
            }

            ws.Cells["A36:B36"].Merge = true;
            ws.Cells["A36:B36"].Style.Font.Bold = true;
            ws.Cells["A36:B36"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A36"].Value = "Секция запаси";
            ws.Cells["A37"].Value = "От мед:"; ws.Cells["B37"].Value = inspection.StoredHoney;
            ws.Cells["A38"].Value = "От прашец:"; ws.Cells["B38"].Value = inspection.StoredPollen;

            if (!inspection.IncludeSpottedProblem)
            {
                for (int i = 40; i < 45; i++)
                {
                    ws.Row(i).Hidden = true;
                }
            }

            ws.Cells["A40:B40"].Merge = true;
            ws.Cells["A40:B40"].Style.Font.Bold = true;
            ws.Cells["A40:B40"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A40"].Value = "Секция проблеми";
            ws.Cells["A41"].Value = "Проблем:"; ws.Cells["B41"].Value = inspection.Disease;
            ws.Cells["A42"].Value = "Третиране с:"; ws.Cells["B42"].Value = inspection.Treatment;
            ws.Cells["A43"].Value = "Вредители:"; ws.Cells["B43"].Value = inspection.Pests;
            ws.Cells["A44"].Value = "Хищници:"; ws.Cells["B44"].Value = inspection.Predators;

            if (!inspection.IncludeWeatherInfo)
            {
                for (int i = 46; i < 50; i++)
                {
                    ws.Row(i).Hidden = true;
                }
            }

            ws.Cells["A46:B46"].Merge = true;
            ws.Cells["A46:B46"].Style.Font.Bold = true;
            ws.Cells["A46:B46"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A46"].Value = "Секция метеорологични";
            ws.Cells["A47"].Value = "Условия";
            ws.Cells["B47"].Value = inspection.Conditions;
            ws.Cells["A48"].Value = "Температура(t°):";
            ws.Cells["B48"].Value = inspection.WeatherTemperature;
            ws.Cells["A49"].Value = "Влажност(%):";
            ws.Cells["B49"].Value = inspection.WeatherHumidity;

            ws.Cells["A51:B51"].Merge = true;
            ws.Cells["A51:B51"].Style.Font.Bold = true;
            ws.Cells["A51:B51"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A51"].Value = "Бележка";

            ws.Cells["A52:B52"].Merge = true;
            ws.Cells["A52:B52"].Style.Font.Bold = true;
            ws.Cells["A52:B52"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells["A52:B52"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells["A52"].Value = inspection.Note;

            ws.Cells["B:B"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            ws.Cells["A:AZ"].AutoFitColumns();

            this.Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            return new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
