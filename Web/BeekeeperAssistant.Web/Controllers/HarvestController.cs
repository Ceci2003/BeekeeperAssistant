namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using BeekeeperAssistant.Web.ViewModels.Harvest;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;

    public class HarvestController : BaseController
    {
        private readonly IHarvestService harvestService;
        private readonly IBeehiveService beehiveService;
        private readonly UserManager<ApplicationUser> userManager;

        public HarvestController(
            IHarvestService harvestService,
            IBeehiveService beehiveService,
            UserManager<ApplicationUser> userManager)
        {
            this.harvestService = harvestService;
            this.beehiveService = beehiveService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = new AllHarvestsViewModel
            {
                AllHarvests = this.harvestService.GetAllUserHarvests<HarvestDatavVewModel>(user.Id),
            };

            return this.View(viewModel);
        }

        public IActionResult ById(int id)
        {
            var viewModel = this.harvestService.GetHarvestById<HarvestDatavVewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        public IActionResult Create(int id)
        {
            var inputModel = new CreateHarvestInputModel
            {
                BeehiveId = id,
                DateOfHarves = DateTime.Now.Date,
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateHarvestInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var harvestId = await this.harvestService.
                CreateUserHarvestAsync(
                    user.Id,
                    inputModel.BeehiveId,
                    inputModel.HarvestName,
                    inputModel.DateOfHarves,
                    inputModel.Product,
                    inputModel.HoneyType,
                    inputModel.Note,
                    inputModel.Amount);

            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataViewModel>(inputModel.BeehiveId);

            return this.Redirect($"/Beehive/{beehive.ApiaryNumber}/{beehive.Id}");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var inputModel = this.harvestService.GetHarvestById<EditHarvestInputModel>(id);
            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataViewModel>(inputModel.BeehiveId);

            await this.harvestService.DeleteHarvestAsync(id);

            return this.Redirect($"/Beehive/{beehive.ApiaryNumber}/{beehive.Id}");
        }

        public IActionResult Edit(int id)
        {
            var inputModel = this.harvestService.GetHarvestById<EditHarvestInputModel>(id);
            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditHarvestInputModel inputModel)
        {
            var harvestId = await this.harvestService.EditHarvestAsync(
                id,
                inputModel.HarvestName,
                inputModel.DateOfHarves,
                inputModel.Product,
                inputModel.HoneyType,
                inputModel.Note,
                inputModel.Amount);

            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataViewModel>(inputModel.BeehiveId);

            return this.Redirect($"/Beehive/{beehive.ApiaryNumber}/{beehive.Id}");
        }

        public async Task<IActionResult> ExportToExcel(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataViewModel>(id);

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1:B1"].Merge = true;
            ws.Cells["A1"].Value = $"Доклад - Добиви кошер";
            ws.Cells["A2:B2"].Merge = true;
            ws.Cells["A2"].Value = $"Дата: {string.Format("{0:dd-MM-yyyy} {0:H:mm}", DateTimeOffset.Now)}";

            ws.Cells["A3"].Value = "Номер:";
            ws.Cells["A4"].Value = "Пчелин:";
            ws.Cells["A5"].Value = "Дата на създаване:";
            ws.Cells["A6"].Value = "Сила:";
            ws.Cells["A7"].Value = "Система:";
            ws.Cells["A8"].Value = "Вид:";
            ws.Cells["A9"].Value = "Апарат за майки:";
            ws.Cells["A10"].Value = "Прашецоуловител:";
            ws.Cells["A11"].Value = "Решетка за прополис:";
            ws.Cells["A12"].Value = "Кралица:";
            ws.Cells["A13"].Value = "Цвят:";
            ws.Cells["A14"].Value = "Вид";
            ws.Cells["A15"].Value = "Дата на придаване:";
            ws.Cells["A16"].Value = "Произход:";

            ws.Cells["B3"].Value = beehive.Number;
            ws.Cells["B4"].Value = beehive.ApiaryNumber;
            ws.Cells["B5"].Value = beehive.Date;
            ws.Cells["B6"].Value = beehive.BeehivePower;
            ws.Cells["B7"].Value = beehive.BeehiveSystem;
            ws.Cells["B8"].Value = beehive.BeehiveType;
            ws.Cells["B9"].Value = beehive.HasDevice == true ? "Има" : "Няма";
            ws.Cells["B10"].Value = beehive.HasPolenCatcher == true ? "Има" : "Няма";
            ws.Cells["B11"].Value = beehive.HasPropolisCatcher == true ? "Има" : "Няма";
            if (beehive.HasQueen)
            {
                ws.Cells["B12"].Value = "Нама";
                ws.Cells["B12"].Style.Font.Bold = true;
                ws.Cells["B13"].Value = "-";
                ws.Cells["B14"].Value = "-";
                ws.Cells["B15"].Value = "-";
                ws.Cells["B16"].Value = "-";
            }
            else
            {
                ws.Cells["B12"].Value = beehive.HasQueen;
                ws.Cells["B13"].Value = beehive.Queen.Color;
                ws.Cells["B14"].Value = beehive.Queen.QueenType;
                ws.Cells["B15"].Value = beehive.Queen.GivingDate;
                ws.Cells["B16"].Value = beehive.Queen.Origin;
            }

            ws.Cells["A18"].Value = "Име";
            ws.Cells["B18"].Value = "Дата";
            ws.Cells["C18"].Value = "Продукт";
            ws.Cells["D18"].Value = "Количество";
            ws.Cells["E18"].Value = "Бележка";

            ws.Cells["A18:E18"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A18:E18"].Style.Fill.BackgroundColor.SetColor(1, 183, 225, 205);
            ws.Cells["A18:E18"].Style.Font.Color.SetColor(Color.White);

            int rowIndex = 19;
            foreach (var harvest in beehive.Harvests)
            {
                ws.Cells[$"A{rowIndex}"].Value = harvest.HarvestName;
                ws.Cells[$"B{rowIndex}"].Value = harvest.DateOfHarves;
                ws.Cells[$"C{rowIndex}"].Value = harvest.Product == "Мед" ? $"Мед - {harvest.Product}" : $"{harvest.Product}";
                ws.Cells[$"D{rowIndex}"].Value = harvest.Amount;
                ws.Cells[$"E{rowIndex}"].Value = harvest.Note;

                rowIndex++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();

            this.Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            return new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
