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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHarvestService harvestService;
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;

        public HarvestController(
            UserManager<ApplicationUser> userManager,
            IHarvestService harvestService,
            IApiaryService apiaryService,
            IBeehiveService beehiveService)
        {
            this.userManager = userManager;
            this.harvestService = harvestService;
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
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

        public async Task<IActionResult> Create(int? beehiveId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var inputModel = new CreateHarvestInputModel
            {
                DateOfHarves = DateTime.Now.Date,
            };

            if (beehiveId == null)
            {
                inputModel.Apiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
            }
            else
            {
                inputModel.ApiaryId = this.apiaryService.GetApiaryIdByBeehiveId(beehiveId.Value);
                inputModel.BeehiveId = beehiveId.Value;
            }

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateHarvestInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                inputModel.DateOfHarves = DateTime.UtcNow.Date;
                if (inputModel.BeehiveId == null)
                {
                    inputModel.Apiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
                }

                return this.View(inputModel);
            }

            if (inputModel.BeehiveId == null)
            {
                var apiaryBeehives = this.beehiveService.GetApiaryBeehivesById<BeehiveViewModel>(inputModel.ApiaryId).ToList();
                if (inputModel.AllBeehives)
                {
                    var beehiveIds = apiaryBeehives.Select(b => b.Id).ToList();
                    await this.harvestService.CreateUserHarvestAsync(currentUser.Id, inputModel, beehiveIds);
                }
                else
                {
                    var selectedIds = new List<int>();
                    var selectedBeehivesNumbers = inputModel.BeehiveNumbersSpaceSeparated.Split(' ').Select(n => Convert.ToInt32(n)).ToList();
                    foreach (var number in selectedBeehivesNumbers)
                    {
                        var beehive = apiaryBeehives.FirstOrDefault(b => b.Number == number);
                        if (beehive != null)
                        {
                            selectedIds.Add(beehive.Id);
                        }
                    }

                    await this.harvestService.CreateUserHarvestAsync(currentUser.Id, inputModel, selectedIds);
                }

                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                await this.harvestService.CreateUserHarvestAsync(currentUser.Id, inputModel, new List<int> { inputModel.BeehiveId.Value });

                return this.RedirectToAction("ById", "Beehive", new { beehiveId = inputModel.BeehiveId.Value, tabPage = "Harvests" });
            }
        }

        public IActionResult Edit(int id, int beehiveId)
        {
            var inputModel = this.harvestService.GetHarvestById<EditHarvestInputModel>(id);
            inputModel.QuantityText = inputModel.Quantity.ToString();
            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditHarvestInputModel inputModel)
        {
            inputModel.Quantity = Convert.ToDouble(inputModel.QuantityText);

            await this.harvestService.EditHarvestAsync(id, inputModel);

            return this.RedirectToAction("ById", "Beehive", new { beehiveId = inputModel.BeehiveId.Value, tabPage = "Harvests" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, int beehiveId)
        {
            var currentuser = await this.userManager.GetUserAsync(this.User);
            var inputModel = this.harvestService.GetHarvestById<HarvestDatavVewModel>(id);

            if (inputModel.CreatorId != currentuser.Id)
            {
                return this.BadRequest();
            }

            await this.harvestService.DeleteHarvestAsync(id);

            return this.RedirectToAction("ById", "Beehive", new { beehiveId = beehiveId, tabPage = "Harvests" });
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
                ws.Cells[$"C{rowIndex}"].Value = harvest.HarvestProductType.ToString();
                ws.Cells[$"D{rowIndex}"].Value = harvest.Quantity;
                ws.Cells[$"E{rowIndex}"].Value = harvest.Note;

                rowIndex++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();

            this.Response.Headers.Add("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            return new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
