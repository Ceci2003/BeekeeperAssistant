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
        private readonly IBeehiveHelperService beehiveHelperService;

        public TreatmentController(
            UserManager<ApplicationUser> userManager,
            ITreatmentService treatmentService,
            IApiaryService apiaryService,
            IBeehiveService beehiveService,
            IExcelExportService excelExportService,
            IBeehiveHelperService beehiveHelperService)
        {
            this.userManager = userManager;
            this.treatmentService = treatmentService;
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
            this.excelExportService = excelExportService;
            this.beehiveHelperService = beehiveHelperService;
        }

        public async Task<IActionResult> AllByBeehiveId(int id, int page = 1)
        {
            if (page <= 0)
            {
                page = 1;
            }

            var viewModel = new AllByBeehiveIdTreatementViewModel()
            {
                AllTreatements =
                    this.treatmentService.GetAllBeehiveTreatments<TreatmentDataViewModel>(id, GlobalConstants.ApiariesPerPage, (page - 1) * GlobalConstants.ApiariesPerPage),
            };

            var currentUser = await this.userManager.GetUserAsync(this.User);
            viewModel.BeehiveAccess = await this.beehiveHelperService.GetUserBeehiveAccessAsync(currentUser.Id, id);
            viewModel.BeehiveId = id;
            viewModel.BeehiveNumber = this.beehiveService.GetBeehiveNumberById(id);
            viewModel.ApiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(id);

            var count = this.treatmentService.GetBeehiveTreatmentsCountByBeehiveId(id);
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
                return this.RedirectToAction("AllByBeehiveId", "Treatment", new { id = id.Value });
            }
        }

        public IActionResult Edit(int id)
        {
            var inputModel = this.treatmentService.GetTreatmentById<EditTreatmentInputModel>(id);

            var beehiveId = this.beehiveService.GetBeehiveIdByTreatmentId(id);
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
            return this.RedirectToAction("AllByBeehiveId", "Treatment", new { id = inputModel.BeehiveId.Value });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var treatment = this.treatmentService.GetTreatmentById<TreatmentDataViewModel>(id);
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (treatment.CreatorId != currentUser.Id)
            {
                return this.BadRequest();
            }

            await this.treatmentService.DeleteTreatmentAsync(id);

            var beehiveId = this.beehiveService.GetBeehiveIdByTreatmentId(id);

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно изтрито третиране!";
            return this.RedirectToAction("AllByBeehiveId", "Treatment", new { id = beehiveId });
        }

        public IActionResult ExportToExcel(int id)
        {
            var pck = this.excelExportService.ExportAsExcelTreatment(id);

            this.Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            return new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
