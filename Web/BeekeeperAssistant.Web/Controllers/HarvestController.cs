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
            var beehive = this.beehiveService.GetBeehiveById<BeehiveViewModel>(id);
            var harvests = this.harvestService.GetAllBeehiveHarvests<HarvestDatavVewModel>(id);

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1:B1"].Merge = true;
            ws.Cells["A1"].Value = $"Доклад - Добиви";

            ws.Cells["A2"].Value = $"Пчелин №:";
            ws.Cells["B2"].Value = $"{beehive.ApiaryNumber}";
            ws.Cells["A3"].Value = $"Кошер №:";
            ws.Cells["B3"].Value = $"{beehive.Number}";

            ws.Cells["A4:B4"].Merge = true;
            ws.Cells["A4"].Value = $"Дата: {string.Format("{0:dd-MM-yyyy} {0:H:mm}", DateTimeOffset.Now)}";

            ws.Cells[$"A6"].Value = $"Име";
            ws.Cells[$"B6"].Value = $"Дата";
            ws.Cells[$"C6"].Value = $"Добит продукт";
            ws.Cells[$"D6"].Value = $"Вид мед";
            ws.Cells[$"E6"].Value = $"Количество";
            ws.Cells[$"F6"].Value = $"Еденица";
            ws.Cells[$"G6"].Value = $"Бележка";

            ws.Cells["A6:G6"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A6:G6"].Style.Fill.BackgroundColor.SetColor(1, 183, 225, 205);
            ws.Cells["A6:G6"].Style.Font.Color.SetColor(Color.White);
            ws.Cells["A6:G6"].Style.Font.Bold = true;

            int rowsCounter = 7;
            foreach (var harvest in harvests)
            {
                ws.Cells[$"A{rowsCounter}"].Value = harvest.HarvestName;
                ws.Cells[$"B{rowsCounter}"].Value = harvest.DateOfHarves.ToString("dd-MM-yyyy");

                var harvestedProduct = "мед";
                switch (harvest.HarvestProductType)
                {
                    case HarvestProductType.Pollen:
                        harvestedProduct = "прашец";
                        break;
                    case HarvestProductType.Wax:
                        harvestedProduct = "восък";
                        break;
                    case HarvestProductType.Propolis:
                        harvestedProduct = "прополис";
                        break;
                    case HarvestProductType.RoyalJelly:
                        harvestedProduct = "млечице";
                        break;
                    case HarvestProductType.BeeVenom:
                        harvestedProduct = "отрова";
                        break;
                }

                ws.Cells[$"C{rowsCounter}"].Value = harvestedProduct;

                var honeyType = string.Empty;
                if (harvest.HarvestProductType == HarvestProductType.Honey)
                {
                    switch (harvest.HoneyType)
                    {
                        case HoneyType.Acacia:
                            honeyType = "акация";
                            break;
                        case HoneyType.Wildflower:
                            honeyType = "билков";
                            break;
                        case HoneyType.Sunflower:
                            honeyType = "слънчогледов";
                            break;
                        case HoneyType.Clover:
                            honeyType = "от детелина";
                            break;
                        case HoneyType.Alfalfa:
                            honeyType = "от люцерна";
                            break;
                        case HoneyType.Other:
                            honeyType = "друг";
                            break;
                    }
                }

                ws.Cells[$"D{rowsCounter}"].Value = honeyType;
                ws.Cells[$"E{rowsCounter}"].Value = harvest.Quantity;

                var unit = "";
                switch (harvest.Unit)
                {
                    case Unit.Kilograms:
                        unit = "кг";
                        break;
                    case Unit.Grams:
                        unit = "г";
                        break;
                    case Unit.Milligrams:
                        unit = "мг";
                        break;
                    case Unit.Litres:
                        unit = "л";
                        break;
                    case Unit.Millilitres:
                        unit = "мл";
                        break;
                }

                ws.Cells[$"F{rowsCounter}"].Value = unit;
                ws.Cells[$"G{rowsCounter}"].Value = harvest.Note;

                rowsCounter++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();

            this.Response.Headers.Add("content-disposition", "attachment: filename=" + $"{beehive.Number}_{beehive.ApiaryNumber}_Harvests.xlsx");
            return new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
