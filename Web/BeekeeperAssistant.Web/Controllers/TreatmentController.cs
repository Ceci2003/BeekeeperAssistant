namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using BeekeeperAssistant.Web.ViewModels.Treatments;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;

    [Authorize]
    public class TreatmentController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITreatmentService treatmentService;
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;

        public TreatmentController(
            UserManager<ApplicationUser> userManager,
            ITreatmentService treatmentService,
            IApiaryService apiaryService,
            IBeehiveService beehiveService)
        {
            this.userManager = userManager;
            this.treatmentService = treatmentService;
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
        }

        public async Task<IActionResult> Create(int? id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var inputModel = new CreateTreatmentInputModel()
            {
                DateOfTreatment = DateTime.UtcNow.Date,
            };

            if (id == null)
            {
                inputModel.Apiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
            }
            else
            {
                inputModel.ApiaryId = this.apiaryService.GetApiaryIdByBeehiveId(id.Value);
                inputModel.BeehiveId = id.Value;
            }

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, CreateTreatmentInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                inputModel.DateOfTreatment = DateTime.UtcNow.Date;
                if (id == null)
                {
                    inputModel.Apiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
                }

                return this.View(inputModel);
            }

            var apiaryNumber = this.apiaryService.GetApiaryById<ApiaryViewModel>(inputModel.ApiaryId).Number;

            if (id == null)
            {
                var apiaryBeehives = this.beehiveService.GetApiaryBeehivesById<BeehiveViewModel>(inputModel.ApiaryId).ToList();
                if (inputModel.AllBeehives)
                {
                    var beehivesIds = apiaryBeehives.Select(b => b.Id).ToList();
                    await this.treatmentService.CreateTreatment(
                    currentUser.Id,
                    inputModel.DateOfTreatment,
                    inputModel.Name,
                    inputModel.Note,
                    inputModel.Disease,
                    inputModel.Medication,
                    inputModel.InputAs,
                    inputModel.Quantity,
                    inputModel.Dose,
                    beehivesIds);
                }
                else
                {
                    var selectedIds = new List<int>();
                    var selectedBeehiveNumbers = inputModel.BeehiveNumbersSpaceSeparated.Split(' ').Select(n => Convert.ToInt32(n)).ToList();
                    foreach (var number in selectedBeehiveNumbers)
                    {
                        var beehive = apiaryBeehives.Where(b => b.Number == number).FirstOrDefault();
                        if (beehive != null)
                        {
                            selectedIds.Add(beehive.Id);
                        }
                    }

                    await this.treatmentService.CreateTreatment(
                            currentUser.Id,
                            inputModel.DateOfTreatment,
                            inputModel.Name,
                            inputModel.Note,
                            inputModel.Disease,
                            inputModel.Medication,
                            inputModel.InputAs,
                            inputModel.Quantity,
                            inputModel.Dose,
                            selectedIds);
                }

                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                await this.treatmentService.CreateTreatment(
                currentUser.Id,
                inputModel.DateOfTreatment,
                inputModel.Name,
                inputModel.Note,
                inputModel.Disease,
                inputModel.Medication,
                inputModel.InputAs,
                inputModel.Quantity,
                inputModel.Dose,
                new List<int> { id.Value });

                return this.RedirectToAction("ById", "Beehive", new { beehiveId = id.Value, tabPage = "Treatments" });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var inputModel = this.treatmentService.GetTreatmentById<EditTreatmentInputModel>(id);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditTreatmentInputModel inputModel)
        {
            // ToDo: make quantity string
            // var quantity = Convert.ToDouble(inputModel.Quantity);

            await this.treatmentService.EditTreatment(
                id,
                inputModel.BeehiveId.Value,
                inputModel.DateOfTreatment,
                inputModel.Name,
                inputModel.Note,
                inputModel.Disease,
                inputModel.Medication,
                inputModel.InputAs,
                inputModel.Quantity,
                inputModel.Dose);

            return this.RedirectToAction("ById", "Beehive", new { beehiveId = inputModel.BeehiveId.Value, tabPage = "Treatments" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int? beehiveId)
        {
            var treatment = this.treatmentService.GetTreatmentById<TreatmentDataViewModel>(id);
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (treatment.CreatorId != currentUser.Id)
            {
                return this.BadRequest();
            }

            await this.treatmentService.DeleteTreatmentAsync(id);

            return this.RedirectToAction("ById", "Beehive", new { beehiveId = beehiveId, tabPage = "Treatments" });
        }

        public async Task<IActionResult> ExportToExcel(int id)
        {
            var beehive = this.beehiveService.GetBeehiveById<BeehiveViewModel>(id);
            var treatments = this.treatmentService.GetAllBeehiveTreatments<TreatmentDataViewModel>(id);

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1:B1"].Merge = true;
            ws.Cells["A1"].Value = $"Доклад - Третирания";

            ws.Cells["A2"].Value = $"Пчелин №:";
            ws.Cells["B2"].Value = $"{beehive.ApiaryNumber}";
            ws.Cells["A3"].Value = $"Кошер №:";
            ws.Cells["B3"].Value = $"{beehive.Number}";

            ws.Cells["A4:B4"].Merge = true;
            ws.Cells["A4"].Value = $"Дата: {string.Format("{0:dd-MM-yyyy} {0:H:mm}", DateTimeOffset.Now)}";

            ws.Cells[$"A6"].Value = $"Име";
            ws.Cells[$"B6"].Value = $"Дата";
            ws.Cells[$"C6"].Value = $"Превенция на";
            ws.Cells[$"D6"].Value = $"Препарат";
            ws.Cells[$"E6"].Value = $"Въведен като";
            ws.Cells[$"F6"].Value = $"Количество";
            ws.Cells[$"G6"].Value = $"Дозировка";

            ws.Cells["A6:G6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A6:G6"].Style.Fill.BackgroundColor.SetColor(1, 183, 225, 205);
            ws.Cells["A6:G6"].Style.Font.Color.SetColor(Color.White);
            ws.Cells["A6:G6"].Style.Font.Bold = true;

            int rowsCounter = 7;
            foreach (var treatment in treatments)
            {
                ws.Cells[$"A{rowsCounter}"].Value = treatment.Name;
                ws.Cells[$"B{rowsCounter}"].Value = treatment.DateOfTreatment.ToString("dd-MM-yyyy");
                ws.Cells[$"C{rowsCounter}"].Value = treatment.Disease;
                ws.Cells[$"D{rowsCounter}"].Value = treatment.Medication;

                var inputAs = treatment.InputAs.ToString();
                switch (treatment.InputAs)
                {
                    case InputAs.PerHive:
                        inputAs = "на кошер";
                        break;
                    case InputAs.Total:
                        inputAs = "общо";
                        break;
                }

                ws.Cells[$"E{rowsCounter}"].Value = inputAs;
                ws.Cells[$"F{rowsCounter}"].Value = treatment.Quantity;

                var dose = treatment.Dose.ToString();
                switch (treatment.Dose)
                {
                    case Dose.Strips:
                        dose = "ленти";
                        break;
                    case Dose.Drops:
                        dose = "капки";
                        break;
                    case Dose.Grams:
                        dose = "г";
                        break;
                    case Dose.Kilograms:
                        dose = "кг";
                        break;
                    case Dose.Millilitres:
                        dose = "мл";
                        break;
                    case Dose.Litres:
                        dose = "л";
                        break;
                }

                ws.Cells[$"G{rowsCounter}"].Value = dose;

                rowsCounter++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();

            this.Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            return new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
