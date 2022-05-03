﻿namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.ApiaryHelpers;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    [Authorize]
    public class ApiaryController : AppBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;
        private readonly IApiaryNumberService apiaryNumberService;
        private readonly IConfiguration configuration;
        private readonly IForecastService forecastService;
        private readonly IApiaryHelperService apiaryHelperService;
        private readonly IBeehiveHelperService beehiveHelperService;
        private readonly ITemporaryApiaryBeehiveService temporaryApiaryBeehiveService;
        private readonly IExcelExportService excelExportService;

        public ApiaryController(
            UserManager<ApplicationUser> userManager,
            IApiaryService apiaryService,
            IBeehiveService beehiveService,
            IApiaryNumberService apiaryNumberService,
            IConfiguration configuration,
            IForecastService forecastService,
            IApiaryHelperService apiaryHelperService,
            IBeehiveHelperService beehiveHelperService,
            ITemporaryApiaryBeehiveService temporaryApiaryBeehiveService,
            IExcelExportService excelExportService)
        {
            this.userManager = userManager;
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
            this.apiaryNumberService = apiaryNumberService;
            this.configuration = configuration;
            this.forecastService = forecastService;
            this.apiaryHelperService = apiaryHelperService;
            this.beehiveHelperService = beehiveHelperService;
            this.temporaryApiaryBeehiveService = temporaryApiaryBeehiveService;
            this.excelExportService = excelExportService;
        }

        public async Task<IActionResult> All(
            int pageAllApiaries = 1,
            string allApieariesOrderBy = null,
            int pageHelperApiaries = 1,
            string helperApiariesOrderBy = null)
        {
            if (pageAllApiaries <= 0)
            {
                pageAllApiaries = 1;
            }

            if (pageHelperApiaries <= 0)
            {
                pageHelperApiaries = 1;
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            var userApiariesCount = this.apiaryService.GetAllUserApiariesCount(currentUser.Id);
            var pagesApiaryCount = (int)Math.Ceiling((double)userApiariesCount / GlobalConstants.ApiariesPerPage);

            var apiaryHelperCount = this.apiaryHelperService.GetUserHelperApiariesCount(currentUser.Id);
            var pagesApiaryHelperCount = (int)Math.Ceiling((double)apiaryHelperCount / GlobalConstants.ApiaryHelpersApiaryPerPage);

            var viewModel = new AllApiaryViewModel
            {
                UserApiaries = new AllApiaryUserApiariesViewModel
                {
                    AllUserApiaries = this.apiaryService.GetAllUserApiaries<AllApiaryUserApiariesDataViewModel>(
                        currentUser.Id,
                        GlobalConstants.ApiariesPerPage,
                        (pageAllApiaries - 1) * GlobalConstants.ApiariesPerPage,
                        allApieariesOrderBy),
                    PagesCount = pagesApiaryCount,
                },
                UserHelperApiaries = new AllApiaryUserHelperApiariesViewModel
                {
                    AllUserHelperApiaries = this.apiaryHelperService.GetUserHelperApiaries<AllApiaryUserHelperApiariesDataViewModel>(
                        currentUser.Id,
                        GlobalConstants.ApiaryHelpersApiaryPerPage,
                        (pageHelperApiaries - 1) * GlobalConstants.ApiaryHelpersApiaryPerPage),
                    PagesCount = pagesApiaryHelperCount,
                },
            };

            foreach (var apiary in viewModel.UserHelperApiaries.AllUserHelperApiaries)
            {
                apiary.Access = await this.apiaryHelperService.GetUserApiaryAccessAsync(currentUser.Id, apiary.ApiaryId);
            }

            if (viewModel.UserApiaries.PagesCount == 0)
            {
                viewModel.UserApiaries.PagesCount = 1;
            }

            if (viewModel.UserHelperApiaries.PagesCount == 0)
            {
                viewModel.UserHelperApiaries.PagesCount = 1;
            }

            viewModel.UserApiaries.CurrentPage = pageAllApiaries;
            viewModel.UserHelperApiaries.CurrentPage = pageHelperApiaries;

            return this.View(viewModel);
        }

        public async Task<IActionResult> AllMovable(int pageAllApiaries = 1, string orderBy = null)
        {
            if (pageAllApiaries <= 0)
            {
                pageAllApiaries = 1;
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            var userApiariesCount = this.apiaryService.GetAllUserApiariesCount(currentUser.Id);
            var pagesApiaryCount = (int)Math.Ceiling((double)userApiariesCount / GlobalConstants.ApiariesPerPage);

            var viewModel = new AllApiaryUserMovableApiariesViewModel
            {
                AllUserMovableApiaries = this.apiaryService.GetAllUserMovableApiaries<AllApiaryUserMovableApiariesDataViewModel>(
                        currentUser.Id,
                        GlobalConstants.ApiariesPerPage,
                        (pageAllApiaries - 1) * GlobalConstants.ApiariesPerPage,
                        orderBy),
                PagesCount = pagesApiaryCount,
            };

            foreach (var apiary in viewModel.AllUserMovableApiaries)
            {
                apiary.BeehivesCount = this.temporaryApiaryBeehiveService.GetApiaryBeehivesCount(apiary.Id);
            }

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = pageAllApiaries;

            return this.View(viewModel);
        }

        public async Task<IActionResult> ById(int id)
        {
            var viewModel = this.apiaryService.GetApiaryById<ByNumberApiaryViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (viewModel.CreatorId != currentUser.Id &&
                !this.apiaryHelperService.IsApiaryHelper(currentUser.Id, viewModel.Id) &&
                !await this.userManager.IsInRoleAsync(currentUser, GlobalConstants.AdministratorRoleName))
            {
                return this.BadRequest();
            }

            viewModel.ApiaryAccess = await this.apiaryHelperService.GetUserApiaryAccessAsync(currentUser.Id, viewModel.Id);

            if (viewModel.Number != null)
            {
                var postcode = viewModel.Number.Split('-')[0];
                viewModel.ForecastResult =
                    await this.forecastService.GetApiaryCurrentWeatherByCityPostcode(postcode, this.configuration["OpenWeatherMap:ApiId"]);

                if (viewModel.ForecastResult == null)
                {
                    viewModel.ForecastResult =
                        await this.forecastService.GetApiaryCurrentWeatherByCityName(viewModel.Adress, this.configuration["OpenWeatherMap:ApiId"]);
                }
            }
            else
            {
                viewModel.ForecastResult =
                    await this.forecastService.GetApiaryCurrentWeatherByCityName(viewModel.Adress, this.configuration["OpenWeatherMap:ApiId"]);
            }

            return this.View(viewModel);
        }

        public IActionResult Create(bool selectMovable = false)
        {
            var inputModel = new CreateApiaryInputModel();
            inputModel.IsRegistered = true;

            if (selectMovable)
            {
                inputModel.ApiaryType = ApiaryType.Movable;
            }

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateApiaryInputModel inputModel, string returnUrl)
        {
            if (inputModel.IsRegistered && inputModel.CityCode != null)
            {
                var postcode = inputModel.CityCode.Split('-')[0];
                if (!this.forecastService.ValidateCityPostcode(postcode, this.configuration["OpenWeatherMap:ApiId"]))
                {
                    this.ModelState.AddModelError(string.Empty, "Не съществува населено място с въведения пощенски код.");
                }
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            var apiaryId =
                await this.apiaryService.CreateUserApiaryAsync(
                    currentUser.Id,
                    inputModel.Number,
                    inputModel.Name,
                    inputModel.ApiaryType,
                    inputModel.Adress,
                    inputModel.IsRegistered,
                    inputModel.IsClosed,
                    inputModel.OpeningDate,
                    inputModel.ClosingDate);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно създаден пчелин!";

            return this.RedirectToAction(nameof(this.ById), new { id = apiaryId });
        }

        public IActionResult Edit(int id)
        {
            var viewModel = this.apiaryService.GetApiaryById<EditApiaryInputModel>(id);

            if (viewModel.IsRegistered)
            {
                viewModel.CityCode = this.apiaryNumberService.GetCityCode(viewModel.Number);
                viewModel.FarmNumber = this.apiaryNumberService.GetFarmNumber(viewModel.Number);
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditApiaryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var modelNumber = this.apiaryNumberService.CreateApiaryNumber(inputModel.CityCode, inputModel.FarmNumber);
            if (!inputModel.IsRegistered)
            {
                modelNumber = null;
            }

            var apiaryId =
                await this.apiaryService.EditApiaryByIdAsync(
                    id,
                    modelNumber,
                    inputModel.Name,
                    inputModel.ApiaryType,
                    inputModel.Adress,
                    inputModel.IsRegistered,
                    inputModel.IsClosed,
                    inputModel.OpeningDate.Value,
                    inputModel.ClosingDate.Value);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно редактиран пчелин!";
            return this.RedirectToAction(nameof(this.ById), new { id = apiaryId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, string returnUrl)
        {
            await this.apiaryService.DeleteApiaryByIdAsync(id);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно изтрит пчелин!";

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> SelectApiaryToAddBeehiveInTemporary(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var inputModel = new SelectApiaryToAddBeehiveInTemporaryInputModel
            {
                TemporaryId = id,
                TemporaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(id),
                TemporaryName = this.apiaryService.GetApiaryNameByApiaryId(id),
                AllApiaries = this.apiaryService.GetUserApiariesWithoutTemporaryAsKeyValuePairs(currentUser.Id),
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public IActionResult SelectApiaryToAddBeehiveInTemporary(int id, SelectApiaryToAddBeehiveInTemporaryInputModel inputModel)
        {
            return this.RedirectToAction(nameof(this.SelectBeehivesToAddInTemporary), new { selectedId = inputModel.SelectedApiaryId, temporaryId = id });
        }

        public async Task<IActionResult> SelectBeehivesToAddInTemporary(int selectedId, int temporaryId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var inputModel = new SelectBeehivesToAddInTemporaryInputModel
            {
                TemporaryId = temporaryId,
                TemporaryNumber = this.apiaryService.GetApiaryNumberByApiaryId(temporaryId),
                TemporaryName = this.apiaryService.GetApiaryNameByApiaryId(temporaryId),
                Beehives = this.beehiveService.GetBeehivesByApiaryIdWithoutInTemporary<SelectBeehiveToAddInTemporaryModel>(selectedId).ToList(),
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> SelectBeehivesToAddInTemporary(int id, SelectBeehivesToAddInTemporaryInputModel inputModel)
        {
            var selectedBeehivesIds = inputModel.Beehives.Where(b => b.IsChecked == true).Select(b => b.Id).ToList();

            await this.temporaryApiaryBeehiveService.AddMultipleBeehiveToApiary(inputModel.TemporaryId, selectedBeehivesIds);

            return this.RedirectToAction("AllByMovableApiaryId", "Beehive", new { id = inputModel.TemporaryId });
        }

        public async Task<IActionResult> RemoveBeehiveFromTemporary(int id, int temporaryId)
        {
            await this.temporaryApiaryBeehiveService.RemoveBeehiveFromTemporaryAsync(id);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно премахнат кошер/и!";

            return this.RedirectToAction("AllByMovableApiaryId", "Beehive", new { id = temporaryId });
        }

        public async Task<IActionResult> ExportToExcel()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var exportResult = this.excelExportService.ExportAsExcelApiary(currentUser.Id);

            this.Response.Headers.Add("content-disposition", "attachment: filename=ExcelReport.xlsx");
            return new FileContentResult(exportResult.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<IActionResult> Bookmark(int id)
        {
            await this.apiaryService.BookmarkApiaryAsync(id);
            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> UpdateMovableStatus(int id)
        {
            var beehivesCount = this.beehiveService.GetAllBeehivesCountByApiaryId(id);

            if (beehivesCount > 0)
            {
                this.TempData[GlobalConstants.ErrorMessage] = $"В подвижния пчелин има кошери които нямат основен пчелин!";
                return this.RedirectToAction("AllByMovableApiaryId", "Beehive", new { id = id });
            }

            await this.apiaryService.UpdateMovableStatus(id);
            return this.RedirectToAction(nameof(this.ById), new { id = id });
        }
    }
}
