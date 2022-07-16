namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Filters.Models;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using BeekeeperAssistant.Web.ViewModels.Treatments;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;

    public class TreatmentController : AppBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITreatmentService treatmentService;
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;
        private readonly IExcelExportService excelExportService;
        private readonly IBeehiveHelperService beehiveHelperService;
        private readonly ITypeService typeService;

        public TreatmentController(
            UserManager<ApplicationUser> userManager,
            ITreatmentService treatmentService,
            IApiaryService apiaryService,
            IBeehiveService beehiveService,
            IExcelExportService excelExportService,
            IBeehiveHelperService beehiveHelperService,
            ITypeService typeService)
        {
            this.userManager = userManager;
            this.treatmentService = treatmentService;
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
            this.excelExportService = excelExportService;
            this.beehiveHelperService = beehiveHelperService;
            this.typeService = typeService;
        }

        public async Task<IActionResult> AllByBeehiveId(int id, FilterModel filterModel, int page = 1)
        {
            if (page <= 0)
            {
                page = 1;
            }

            var viewModel = new AllByBeehiveIdTreatementViewModel()
            {
                AllTreatementsFilterModel = new FilterModel
                {
                    Data = new FilterData
                    {
                        ModelProperties = this.typeService.GetAllTypePropertiesName(typeof(AllByBeehiveIdTreatmentFilterModel)),
                        ModelPropertiesDisplayNames = this.typeService.GetAllTypePropertiesDisplayName(typeof(AllByBeehiveIdTreatmentFilterModel)),
                        PageNumber = page,
                    },
                },
                AllTreatements =
                    this.treatmentService.GetAllBeehiveTreatments<TreatmentDataModel>(id, GlobalConstants.ApiariesPerPage, (page - 1) * GlobalConstants.ApiariesPerPage, filterModel),
            };

            var currentUser = await this.userManager.GetUserAsync(this.User);
            viewModel.BeehiveAccess = await this.beehiveHelperService.GetUserBeehiveAccessAsync(currentUser.Id, id);
            viewModel.BeehiveId = id;
            viewModel.BeehiveNumber = this.beehiveService.GetBeehiveNumberById(id);

            var apiary = this.apiaryService.GetUserApiaryByBeehiveId<ApiaryDataModel>(id);
            viewModel.ApiaryId = apiary.Id;
            viewModel.ApiaryNumber = apiary.Number;
            viewModel.ApiaryName = apiary.Name;

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
                var apiaryId = this.apiaryService.GetApiaryIdByBeehiveId(id.Value);
                var apiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(id.Value);
                var apiaryName = this.apiaryService.GetApiaryNameByBeehiveId(id.Value);
                var beehiveNumber = this.beehiveService.GetBeehiveNumberById(id.Value);

                inputModel.ApiaryId = apiaryId;
                inputModel.ApiaryNumber = apiaryNumber;
                inputModel.ApiaryName = apiaryName;
                inputModel.BeehiveId = id.Value;
                inputModel.BeehiveNumber = beehiveNumber;
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

            var apiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(inputModel.ApiaryId);
            var apiaryOwnerId = this.apiaryService.GetApiaryOwnerIdByApiaryId(inputModel.ApiaryId);

            if (id == null)
            {
                var apiaryBeehives = this.beehiveService.GetBeehivesByApiaryId<BeehiveDataModel>(inputModel.ApiaryId).ToList();
                if (inputModel.AllBeehives)
                {
                    var beehivesIds = apiaryBeehives.Select(b => b.Id).ToList();
                    await this.treatmentService.CreateTreatmentAsync(
                    apiaryOwnerId,
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
                            apiaryOwnerId,
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
                apiaryOwnerId,
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
                return this.RedirectToAction(nameof(this.AllByBeehiveId), new { id = id.Value });
            }
        }

        public IActionResult Edit(int id)
        {
            var inputModel = this.treatmentService.GetTreatmentById<EditTreatmentInputModel>(id);

            var beehiveId = this.beehiveService.GetBeehiveIdByTreatmentId(id);
            var beehiveNumber = this.beehiveService.GetBeehiveNumberById(beehiveId);
            var apiaryId = this.apiaryService.GetApiaryIdByBeehiveId(beehiveId);
            var apiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(beehiveId);
            var apiaryName = this.apiaryService.GetApiaryNameByBeehiveId(beehiveId);

            inputModel.BeehiveId = beehiveId;
            inputModel.BeehiveNumber = beehiveNumber;
            inputModel.ApiaryId = apiaryId;
            inputModel.ApiaryNumber = apiaryNumber;
            inputModel.ApiaryName = apiaryName;

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
            return this.RedirectToAction(nameof(this.AllByBeehiveId), new { id = inputModel.BeehiveId.Value });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var treatment = this.treatmentService.GetTreatmentById<TreatmentDataModel>(id);
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (treatment.CreatorId != currentUser.Id)
            {
                return this.BadRequest();
            }

            await this.treatmentService.DeleteTreatmentAsync(id);

            var beehiveId = this.beehiveService.GetBeehiveIdByTreatmentId(id);

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно изтрито третиране!";
            return this.RedirectToAction(nameof(this.AllByBeehiveId), new { id = beehiveId });
        }

        public IActionResult ExportToExcel(int id)
        {
            var pck = this.excelExportService.ExportAsExcelTreatment(id);

            this.Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            return new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
