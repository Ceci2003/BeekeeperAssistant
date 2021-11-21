namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Common;
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
        private readonly IExcelExportService excelExportService;

        public TreatmentController(
            UserManager<ApplicationUser> userManager,
            ITreatmentService treatmentService,
            IApiaryService apiaryService,
            IBeehiveService beehiveService,
            IExcelExportService excelExportService)
        {
            this.userManager = userManager;
            this.treatmentService = treatmentService;
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
            this.excelExportService = excelExportService;
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
                var apiary = this.apiaryService.GetUserApiaryByBeehiveId<ApiaryViewModel>(id.Value);
                var beehive = this.beehiveService.GetBeehiveById<BeehiveViewModel>(id.Value);

                inputModel.ApiaryId = apiary.Id;
                inputModel.BeehiveId = id.Value;
                inputModel.ApiaryNumber = apiary.Number;
                inputModel.BeehiveNumber = beehive.Number;
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
                var apiaryBeehives = this.beehiveService.GetBeehivesByApiaryId<BeehiveViewModel>(inputModel.ApiaryId).ToList();
                if (inputModel.AllBeehives)
                {
                    var beehivesIds = apiaryBeehives.Select(b => b.Id).ToList();
                    await this.treatmentService.CreateTreatmentAsync(
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
                    var selectedBeehiveNumbers = inputModel.BeehiveNumbersSpaceSeparated.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToList();
                    foreach (var number in selectedBeehiveNumbers)
                    {
                        var beehive = apiaryBeehives.Where(b => b.Number == number).FirstOrDefault();
                        if (beehive != null)
                        {
                            selectedIds.Add(beehive.Id);
                        }
                    }

                    await this.treatmentService.CreateTreatmentAsync(
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
                await this.treatmentService.CreateTreatmentAsync(
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

                this.TempData[GlobalConstants.SuccessMessage] = "Успешно създадено третиране!";
                return this.RedirectToAction("ById", "Beehive", new { beehiveId = id.Value, tabPage = "Treatments" });
            }
        }

        public IActionResult Edit(int id, int beehiveId)
        {
            var inputModel = this.treatmentService.GetTreatmentById<EditTreatmentInputModel>(id);

            var apiary = this.apiaryService.GetUserApiaryByBeehiveId<ApiaryViewModel>(beehiveId);
            var beehive = this.beehiveService.GetBeehiveById<BeehiveViewModel>(beehiveId);

            inputModel.BeehiveId = beehiveId;
            inputModel.ApiaryNumber = apiary.Number;
            inputModel.BeehiveNumber = beehive.Number;

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

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно редактирано третиране!";
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

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно изтрито третиране!";
            return this.RedirectToAction("ById", "Beehive", new { beehiveId = beehiveId, tabPage = "Treatments" });
        }

        public IActionResult ExportToExcel(int id)
        {
            var pck = this.excelExportService.ExportAsExcelTreatment(id);

            this.Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            return new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
