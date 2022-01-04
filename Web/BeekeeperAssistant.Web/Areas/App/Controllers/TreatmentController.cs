namespace BeekeeperAssistant.Web.Areas.App.Controllers
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

    public class TreatmentController : AppBaseController
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
                    treatmentService.GetAllBeehiveTreatments<TreatmentDataViewModel>(id, GlobalConstants.ApiariesPerPage, (page - 1) * GlobalConstants.ApiariesPerPage),
            };

            var currentUser = await userManager.GetUserAsync(User);
            viewModel.BeehiveAccess = await beehiveHelperService.GetUserBeehiveAccessAsync(currentUser.Id, id);
            viewModel.BeehiveId = id;
            viewModel.BeehiveNumber = beehiveService.GetBeehiveNumberById(id);

            var apiary = apiaryService.GetUserApiaryByBeehiveId<ApiaryDataModel>(id);
            viewModel.ApiaryId = apiary.Id;
            viewModel.ApiaryNumber = apiary.Number;
            viewModel.ApiaryName = apiary.Name;

            var count = treatmentService.GetBeehiveTreatmentsCountByBeehiveId(id);
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
            var currentUser = await userManager.GetUserAsync(User);

            var inputModel = new CreateTreatmentInputModel()
            {
                DateOfTreatment = DateTime.UtcNow.Date,
            };

            if (id == null)
            {
                inputModel.Apiaries = apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
            }
            else
            {
                var apiary = apiaryService.GetUserApiaryByBeehiveId<ApiaryDataModel>(id.Value);
                var beehiveNumber = beehiveService.GetBeehiveNumberById(id.Value);

                inputModel.ApiaryId = apiary.Id;
                inputModel.BeehiveId = id.Value;
                inputModel.ApiaryNumber = apiary.Number;
                inputModel.BeehiveNumber = beehiveNumber;
            }

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, CreateTreatmentInputModel inputModel)
        {
            var currentUser = await userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                inputModel.DateOfTreatment = DateTime.UtcNow.Date;
                if (id == null)
                {
                    inputModel.Apiaries = apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
                }

                return View(inputModel);
            }

            var apiaryNumber = apiaryService.GetApiaryNumberByApiaryId(inputModel.ApiaryId);
            var apiaryOwnerId = apiaryService.GetApiaryOwnerIdByApiaryId(inputModel.ApiaryId);

            if (id == null)
            {
                var apiaryBeehives = beehiveService.GetBeehivesByApiaryId<BeehiveDataModel>(inputModel.ApiaryId).ToList();
                if (inputModel.AllBeehives)
                {
                    var beehivesIds = apiaryBeehives.Select(b => b.Id).ToList();
                    await treatmentService.CreateTreatmentAsync(
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

                    await treatmentService.CreateTreatmentAsync(
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

                return RedirectToAction("Index", "Home");
            }
            else
            {
                await treatmentService.CreateTreatmentAsync(
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

                TempData[GlobalConstants.SuccessMessage] = "Успешно създадено третиране!";
                return RedirectToAction(nameof(this.AllByBeehiveId), new { id = id.Value });
            }
        }

        public IActionResult Edit(int id)
        {
            var inputModel = treatmentService.GetTreatmentById<EditTreatmentInputModel>(id);

            var beehiveId = beehiveService.GetBeehiveIdByTreatmentId(id);
            var apiaryNumber = apiaryService.GetApiaryNumberByBeehiveId(beehiveId);
            var beehiveNumber = beehiveService.GetBeehiveNumberById(beehiveId);

            inputModel.BeehiveId = beehiveId;
            inputModel.ApiaryNumber = apiaryNumber;
            inputModel.BeehiveNumber = beehiveNumber;

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditTreatmentInputModel inputModel)
        {
            await treatmentService.EditTreatment(
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

            TempData[GlobalConstants.SuccessMessage] = "Успешно редактирано третиране!";
            return RedirectToAction(nameof(this.AllByBeehiveId), new { id = inputModel.BeehiveId.Value });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var treatment = treatmentService.GetTreatmentById<TreatmentDataViewModel>(id);
            var currentUser = await userManager.GetUserAsync(User);

            if (treatment.CreatorId != currentUser.Id)
            {
                return BadRequest();
            }

            await treatmentService.DeleteTreatmentAsync(id);

            var beehiveId = beehiveService.GetBeehiveIdByTreatmentId(id);

            TempData[GlobalConstants.SuccessMessage] = "Успешно изтрито третиране!";
            return RedirectToAction(nameof(this.AllByBeehiveId), new { id = beehiveId });
        }

        public IActionResult ExportToExcel(int id)
        {
            var pck = excelExportService.ExportAsExcelTreatment(id);

            Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            return new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
