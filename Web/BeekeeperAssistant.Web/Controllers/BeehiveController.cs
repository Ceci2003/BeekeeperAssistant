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
    using BeekeeperAssistant.Services.Messaging;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using BeekeeperAssistant.Web.ViewModels.Harvest;
    using BeekeeperAssistant.Web.ViewModels.Inspection;
    using BeekeeperAssistant.Web.ViewModels.Queens;
    using BeekeeperAssistant.Web.ViewModels.Treatments;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;

    [Authorize]
    public class BeehiveController : BaseController
    {
        private readonly IApiaryService apiaryService;
        private readonly IApiaryHelperService apiaryHelperService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBeehiveHelperService beehiveHelperService;
        private readonly IBeehiveService beehiveService;
        private readonly IQueenService queenService;
        private readonly IHarvestService harvestService;
        private readonly ITreatmentService treatmentService;
        private readonly IInspectionService inspectionService;
        private readonly IQueenHelperService queenHelperService;

        public BeehiveController(
            IApiaryService apiaryService,
            IApiaryHelperService apiaryHelperService,
            UserManager<ApplicationUser> userManager,
            IBeehiveHelperService beehiveHelperService,
            IBeehiveService beehiveService,
            IQueenService queenService,
            IHarvestService harvestService,
            ITreatmentService treatmentService,
            IInspectionService inspectionService,
            IQueenHelperService queenHelperService)
        {
            this.apiaryService = apiaryService;
            this.apiaryHelperService = apiaryHelperService;
            this.userManager = userManager;
            this.beehiveHelperService = beehiveHelperService;
            this.beehiveService = beehiveService;
            this.queenService = queenService;
            this.harvestService = harvestService;
            this.treatmentService = treatmentService;
            this.inspectionService = inspectionService;
            this.queenHelperService = queenHelperService;
        }

        public async Task<IActionResult> All(int page = 1)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var allBehhives = this.beehiveService.GetAllUserBeehives<BeehiveViewModel>(currentUser.Id, GlobalConstants.BeehivesPerPage, (page - 1) * GlobalConstants.BeehivesPerPage);

            var viewModel = new AllBeehivesViewModel
            {
                AllBeehives = allBehhives,
            };

            var count = this.beehiveService.GetAllUserBeehivesCount(currentUser.Id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / GlobalConstants.BeehivesPerPage);
            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }

        public async Task<IActionResult> ById(int beehiveId, string tabPage)
        {
            var viewModel = this.beehiveService.GetBeehiveById<BeehiveDataViewModel>(beehiveId);
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var apiaryId = this.apiaryService.GetApiaryIdByBeehiveId(beehiveId);

            if (viewModel.CreatorId != currentUser.Id &&
                !this.apiaryHelperService.IsApiaryHelper(currentUser.Id, apiaryId) &&
                !this.apiaryService.IsApiaryCreator(currentUser.Id, apiaryId))
            {
                return this.BadRequest();
            }

            viewModel.BeehiveAccess = currentUser.Id == viewModel.CreatorId ? Access.ReadWrite : this.beehiveHelperService.GetUserBeehiveAccess(currentUser.Id, viewModel.Id);

            if (viewModel.Queen != null)
            {
                viewModel.QueenAccess = currentUser.Id == viewModel.Queen.UserId ? Access.ReadWrite : this.queenHelperService.GetUserQueenAccess(currentUser.Id, viewModel.QueenId);
            }

            var harvests = this.harvestService.GetAllBeehiveHarvests<HarvestDatavVewModel>(beehiveId);
            viewModel.Harvests = harvests;
            var treatments = this.treatmentService.GetAllBeehiveTreatments<TreatmentDataViewModel>(beehiveId);
            viewModel.Treatments = treatments;
            var inspections = this.inspectionService.GetAllBeehiveInspections<InspectionDataViewModel>(beehiveId);
            viewModel.Inspections = inspections;

            if (string.IsNullOrEmpty(tabPage))
            {
                tabPage = "Beehive";
            }

            viewModel.TabPage = tabPage;

            return this.View(viewModel);
        }

        public async Task<IActionResult> Create(int? id)
        {
            var inputModel = new CreateBeehiveInputModel();

            if (id == null)
            {
                var currentUser = await this.userManager.GetUserAsync(this.User);
                inputModel.AllApiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
            }
            else
            {
                inputModel.ApiaryId = id.Value;
            }

            inputModel.Date = DateTime.UtcNow.Date;

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, CreateBeehiveInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                if (!id.HasValue)
                {
                    inputModel.AllApiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
                }

                return this.View(inputModel);
            }

            var apiaryCreatorId = this.apiaryService.GetApiaryCreatorIdByApiaryId(inputModel.ApiaryId);

            var beehiveId = await this.beehiveService
                .CreateUserBeehiveAsync(
                apiaryCreatorId,
                inputModel.Number,
                inputModel.BeehiveSystem,
                inputModel.BeehiveType,
                inputModel.Date,
                inputModel.ApiaryId,
                inputModel.BeehivePower,
                inputModel.HasDevice,
                inputModel.HasPolenCatcher,
                inputModel.HasPropolisCatcher,
                inputModel.IsItMovable);

            var apiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(beehiveId);
            return this.Redirect($"/Beehive/{apiaryNumber}/{beehiveId}");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var inputModel = this.beehiveService.GetBeehiveById<EditBeehiveInputModel>(id);
            inputModel.AllApiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditBeehiveInputModel inputModel)
        {
            var beehiveId = await this.beehiveService.EditUserBeehiveAsync(
                id,
                inputModel.Number,
                inputModel.BeehiveSystem,
                inputModel.BeehiveType,
                inputModel.Date,
                inputModel.ApiaryId,
                inputModel.BeehivePower,
                inputModel.HasDevice,
                inputModel.HasPolenCatcher,
                inputModel.HasPropolisCatcher,
                inputModel.IsItMovable);

            var apiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(beehiveId);

            return this.Redirect($"/Beehive/{apiaryNumber}/{beehiveId}");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataViewModel>(id);
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (beehive.CreatorId != currentUser.Id)
            {
                return this.BadRequest();
            }

            var apiaryNumber = await this.beehiveService.DeleteBeehiveByIdAsync(id);
            return this.Redirect($"/Apiary/{apiaryNumber}");
        }

        public async Task<IActionResult> ExportToExcel(int? id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            // ToDo: Fix this
            var beehives = this.beehiveService.GetAllUserBeehives<BeehiveDataViewModel>(currentUser.Id);

            if (id != null)
            {
                beehives = this.beehiveService.GetBeehivesByApiaryId<BeehiveDataViewModel>(id.Value);
            }

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1:B1"].Merge = true;
            ws.Cells["A1"].Value = "Доклад - Кошери";
            ws.Cells["A2:B2"].Merge = true;
            ws.Cells["A2"].Value = $"Дата: {string.Format("{0:dd-MM-yyyy} {0:H:mm}", DateTimeOffset.Now)}";

            ws.Cells["A4"].Value = "Номер на кошера";
            ws.Cells["B4"].Value = "Създаден на";
            ws.Cells["C4"].Value = "Номер на пчелин";
            ws.Cells["D4"].Value = "Подвижен";
            ws.Cells["E4"].Value = "Сила";
            ws.Cells["F4"].Value = "Система";
            ws.Cells["G4"].Value = "Тип";
            ws.Cells["H4"].Value = "Прашецоуловител";
            ws.Cells["I4"].Value = "Решетка за прополис";

            ws.Cells["J4"].Value = "Кралица";
            ws.Cells["K4"].Value = "Кралица-Цвят";
            ws.Cells["L4"].Value = "Кралица-Дата на придаване";
            ws.Cells["M4"].Value = "Кралица-Произход";
            ws.Cells["N4"].Value = "Кралица-Вид";
            ws.Cells["O4"].Value = "Кралица-Порода";
            ws.Cells["P4"].Value = "Кралица-Нрав";
            ws.Cells["Q4"].Value = "Кралица-Хигиенни навици";

            ws.Cells["A4:Q4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A4:Q4"].Style.Fill.BackgroundColor.SetColor(1, 183, 225, 205);
            ws.Cells["A4:Q4"].Style.Font.Color.SetColor(Color.White);
            ws.Cells["A4:Q4"].Style.Font.Bold = true;

            int rowIndex = 5;
            foreach (var beehive in beehives)
            {
                ws.Cells[$"A{rowIndex}"].Value = beehive.Number;
                ws.Cells[$"B{rowIndex}"].Value = beehive.Date.ToString("dd-MM-yyyy");
                ws.Cells[$"C{rowIndex}"].Value = beehive.ApiaryNumber;
                ws.Cells[$"D{rowIndex}"].Value = beehive.IsItMovable == true ? "✓	" : string.Empty;

                ws.Cells[$"D{rowIndex}"].Style.Font.Bold = true;

                ws.Cells[$"E{rowIndex}"].Value = beehive.BeehivePower;
                ws.Cells[$"F{rowIndex}"].Value = beehive.BeehiveSystem;
                ws.Cells[$"G{rowIndex}"].Value = beehive.BeehiveType;
                ws.Cells[$"H{rowIndex}"].Value = beehive.HasPolenCatcher == true ? "Да" : "Не";
                ws.Cells[$"I{rowIndex}"].Value = beehive.HasPropolisCatcher == true ? "Да" : "Не";
                if (beehive.Queen != null)
                {
                    // Color color = Color.White;
                    // if (beehive.Queen.Color != )
                    // {
                    //    switch (beehive.Queen.Color)
                    //    {
                    //        case QueenColor.White: color = Color.White; break;
                    //        case QueenColor.Yellow: color = Color.Yellow; break;
                    //        case QueenColor.Red: color = Color.Red; break;
                    //        case QueenColor.Green: color = Color.Green; break;
                    //        case QueenColor.Blue: color = Color.Blue; break;
                    //        default: color = Color.White; break;
                    //    }
                    // }

                    // ws.Cells[$"J{rowIndex}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    // ws.Cells[$"J{rowIndex}"].Style.Fill.BackgroundColor.SetColor(1, 183, 225, 205);
                    ws.Cells[$"J{rowIndex}"].Value = beehive.Queen != null ? "Да" : "Не";
                    ws.Cells[$"K{rowIndex}"].Value = beehive.Queen.Color.ToString();
                    ws.Cells[$"L{rowIndex}"].Value = beehive.Queen.GivingDate.ToString("dd-MM-yyyy");
                    ws.Cells[$"M{rowIndex}"].Value = beehive.Queen.Origin;
                    ws.Cells[$"N{rowIndex}"].Value = beehive.Queen.QueenType;
                    ws.Cells[$"O{rowIndex}"].Value = beehive.Queen.Breed;
                    ws.Cells[$"P{rowIndex}"].Value = beehive.Queen.Temperament;
                    ws.Cells[$"Q{rowIndex}"].Value = beehive.Queen.HygenicHabits;
                }

                rowIndex++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();

            this.Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            return new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<IActionResult> Bookmark(int id)
        {
            var apiaryId = await this.beehiveService.BookmarkBeehiveAsync(id);

            if (!apiaryId.HasValue)
            {
                return this.BadRequest();
            }

            var apiaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(apiaryId.Value);

            return this.Redirect($"/Apiary/{apiaryNumber}");
        }
    }
}
