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
    using BeekeeperAssistant.Web.ViewModels.Harvest;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;

    public class HarvestController : AppBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHarvestService harvestService;
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;
        private readonly IExcelExportService excelExportService;
        private readonly IBeehiveHelperService beehiveHelperService;

        public HarvestController(
            UserManager<ApplicationUser> userManager,
            IHarvestService harvestService,
            IApiaryService apiaryService,
            IBeehiveService beehiveService,
            IExcelExportService excelExportService,
            IBeehiveHelperService beehiveHelperService)
        {
            this.userManager = userManager;
            this.harvestService = harvestService;
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

            var currentUser = await userManager.GetUserAsync(User);

            var viewModel = new AllByBeehiveIdHarvestViewModel()
            {
                AllHarvests =
                    harvestService.GetAllBeehiveHarvests<HarvestDatavVewModel>(id, GlobalConstants.ApiariesPerPage, (page - 1) * GlobalConstants.ApiariesPerPage),
            };

            viewModel.BeehiveId = id;
            viewModel.BeehiveNumber = beehiveService.GetBeehiveNumberById(id);
            viewModel.BeehiveAccess = await beehiveHelperService.GetUserBeehiveAccessAsync(currentUser.Id, id);

            var apiary = apiaryService.GetUserApiaryByBeehiveId<ApiaryDataModel>(id);
            viewModel.ApiaryId = apiary.Id;
            viewModel.ApiaryNumber = apiary.Number;
            viewModel.ApiaryName = apiary.Name;

            var count = harvestService.GetAllBeehiveHarvestsCountByBeehiveId(id);
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

            var inputModel = new CreateHarvestInputModel
            {
                DateOfHarves = DateTime.Now.Date,
            };

            if (id == null)
            {
                inputModel.Apiaries = apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
            }
            else
            {
                var apiaryNumber = apiaryService.GetApiaryNumberByBeehiveId(id.Value);
                var beehiveNumber = beehiveService.GetBeehiveNumberById(id.Value);

                inputModel.ApiaryId = apiaryService.GetApiaryIdByBeehiveId(id.Value);
                inputModel.BeehiveId = id.Value;
                inputModel.ApiaryNumber = apiaryNumber;
                inputModel.BeehiveNumber = beehiveNumber;
            }

            return View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateHarvestInputModel inputModel)
        {
            var currentUser = await userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                inputModel.DateOfHarves = DateTime.UtcNow.Date;
                if (inputModel.BeehiveId == null)
                {
                    inputModel.Apiaries = apiaryService.GetUserApiariesAsKeyValuePairs(currentUser.Id);
                }

                return View(inputModel);
            }

            var apiaryOwnerId = apiaryService.GetApiaryOwnerIdByApiaryId(inputModel.ApiaryId);

            if (inputModel.BeehiveId == null)
            {
                var apiaryBeehives = beehiveService.GetBeehivesByApiaryId<BeehiveDataModel>(inputModel.ApiaryId).ToList();
                if (inputModel.AllBeehives)
                {
                    var beehiveIds = apiaryBeehives.Select(b => b.Id).ToList();
                    await harvestService.CreateUserHarvestAsync(apiaryOwnerId, currentUser.Id, inputModel, beehiveIds);
                }
                else
                {
                    var selectedIds = new List<int>();
                    var selectedBeehivesNumbers = inputModel.BeehiveNumbersSpaceSeparated.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToList();
                    foreach (var number in selectedBeehivesNumbers)
                    {
                        var beehive = apiaryBeehives.FirstOrDefault(b => b.Number == number);
                        if (beehive != null)
                        {
                            selectedIds.Add(beehive.Id);
                        }
                    }

                    await harvestService.CreateUserHarvestAsync(apiaryOwnerId, currentUser.Id, inputModel, selectedIds);
                }

                TempData[GlobalConstants.SuccessMessage] = $"Успешно добавен добив!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                await harvestService.CreateUserHarvestAsync(apiaryOwnerId, currentUser.Id, inputModel, new List<int> { inputModel.BeehiveId.Value });

                TempData[GlobalConstants.SuccessMessage] = $"Успешно добавен добив!";
                return RedirectToAction(nameof(this.AllByBeehiveId), new { id = inputModel.BeehiveId.Value });
            }
        }

        // DONE []
        public IActionResult Edit(int id)
        {
            var inputModel = harvestService.GetHarvestById<EditHarvestInputModel>(id);
            inputModel.QuantityText = inputModel.Quantity.ToString();

            var beehiveId = beehiveService.GetBeehiveIdByHarvesId(id);

            var apiaryNumber = apiaryService.GetApiaryNumberByBeehiveId(beehiveId);
            var beehiveNumber = beehiveService.GetBeehiveNumberById(beehiveId);

            inputModel.BeehiveId = beehiveId;
            inputModel.ApiaryNumber = apiaryNumber;
            inputModel.BeehiveNumber = beehiveNumber;

            return View(inputModel);
        }

        // DONE []
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditHarvestInputModel inputModel)
        {
            inputModel.Quantity = Convert.ToDouble(inputModel.QuantityText);

            await harvestService.EditHarvestAsync(id, inputModel);

            var beehiveId = beehiveService.GetBeehiveIdByHarvesId(id);

            TempData[GlobalConstants.SuccessMessage] = $"Успешно редактиран добив!";
            return RedirectToAction(nameof(this.AllByBeehiveId), new { id = beehiveId });
        }

        // DONE []
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var currentuser = await userManager.GetUserAsync(User);
            var inputModel = harvestService.GetHarvestById<HarvestDatavVewModel>(id);

            if (inputModel.CreatorId != currentuser.Id)
            {
                return BadRequest();
            }

            var beehiveId = beehiveService.GetBeehiveIdByHarvesId(id);

            await harvestService.DeleteHarvestAsync(id);

            TempData[GlobalConstants.SuccessMessage] = $"Успешно изтрит добив!";
            return RedirectToAction(nameof(this.AllByBeehiveId), new { id = beehiveId });
        }

        // DONE []
        public IActionResult ExportToExcel(int id)
        {
            var pck = excelExportService.ExportAsExcelHarvest(id);

            var apiaryNumber = apiaryService.GetApiaryNumberByBeehiveId(id);
            var beehive = beehiveService.GetBeehiveById<ByIdBeehiveViewModel>(id);
            Response.Headers.Add("content-disposition", "attachment: filename=" + $"{beehive.Number}_{apiaryNumber}_Harvests.xlsx");
            return new FileContentResult(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
