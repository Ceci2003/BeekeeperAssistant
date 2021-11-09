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
        private readonly IExcelExportService excelExportService;

        public HarvestController(
            UserManager<ApplicationUser> userManager,
            IHarvestService harvestService,
            IApiaryService apiaryService,
            IBeehiveService beehiveService,
            IExcelExportService excelExportService)
        {
            this.userManager = userManager;
            this.harvestService = harvestService;
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
            this.excelExportService = excelExportService;
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

        // DONE []
        public IActionResult ById(int id)
        {
            var viewModel = this.harvestService.GetHarvestById<HarvestDatavVewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        // DONE []
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
                var apiary = this.apiaryService.GetUserApiaryByBeehiveId<ApiaryViewModel>(beehiveId.Value);
                var beehive = this.beehiveService.GetBeehiveById<BeehiveViewModel>(beehiveId.Value);

                inputModel.ApiaryId = this.apiaryService.GetApiaryIdByBeehiveId(beehiveId.Value);
                inputModel.BeehiveId = beehiveId.Value;
                inputModel.ApiaryNumber = apiary.Number;
                inputModel.BeehiveNumber = beehive.Number;
            }

            return this.View(inputModel);
        }

        // DONE []
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
                var apiaryBeehives = this.beehiveService.GetBeehivesByApiaryId<BeehiveViewModel>(inputModel.ApiaryId).ToList();
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

                this.TempData[GlobalConstants.SuccessMessage] = $"Успешно добавен добив!";
                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                await this.harvestService.CreateUserHarvestAsync(currentUser.Id, inputModel, new List<int> { inputModel.BeehiveId.Value });

                this.TempData[GlobalConstants.SuccessMessage] = $"Успешно добавен добив!";
                return this.RedirectToAction("ById", "Beehive", new { beehiveId = inputModel.BeehiveId.Value, tabPage = "Harvests" });
            }
        }

        // DONE []
        public IActionResult Edit(int id, int beehiveId)
        {
            var inputModel = this.harvestService.GetHarvestById<EditHarvestInputModel>(id);
            inputModel.QuantityText = inputModel.Quantity.ToString();

            var apiary = this.apiaryService.GetUserApiaryByBeehiveId<ApiaryViewModel>(beehiveId);
            var beehive = this.beehiveService.GetBeehiveById<BeehiveViewModel>(beehiveId);

            inputModel.BeehiveId = beehiveId;
            inputModel.ApiaryNumber = apiary.Number;
            inputModel.BeehiveNumber = beehive.Number;

            return this.View(inputModel);
        }

        // DONE []
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditHarvestInputModel inputModel)
        {
            inputModel.Quantity = Convert.ToDouble(inputModel.QuantityText);

            await this.harvestService.EditHarvestAsync(id, inputModel);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно редактиран добив!";
            return this.RedirectToAction("ById", "Beehive", new { beehiveId = inputModel.BeehiveId.Value, tabPage = "Harvests" });
        }

        // DONE []
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

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно изтрит добив!";
            return this.RedirectToAction("ById", "Beehive", new { beehiveId = beehiveId, tabPage = "Harvests" });
        }

        // DONE []
        public IActionResult ExportToExcel(int id)
        {
            var pck = this.excelExportService.ExportAsExcelHarvest(id);

            var apiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(id);
            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataViewModel>(id);
            this.Response.Headers.Add("content-disposition", "attachment: filename=" + $"{beehive.Number}_{apiaryNumber}_Harvests.xlsx");
            return new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
