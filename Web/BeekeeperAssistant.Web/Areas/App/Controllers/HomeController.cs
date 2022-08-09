namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Services.Messaging;
    using BeekeeperAssistant.Web.ViewModels;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using BeekeeperAssistant.Web.ViewModels.Home;
    using BeekeeperAssistant.Web.ViewModels.Queens;
    using BeekeeperAssistant.Web.ViewModels.SystemNotification;
    using BeekeeperAssistant.Web.ViewModels.UserTasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    public class HomeController : AppBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;
        private readonly IQueenService queenService;
        private readonly ITreatmentService treatmentService;
        private readonly IInspectionService inspectionService;
        private readonly IHarvestService harvestService;
        private readonly IQuickChartService quickChartService;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;
        private readonly ISystemNotificationService systemNotificationService;
        private readonly IUserTaskService userTaskService;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IApiaryService apiaryService,
            IBeehiveService beehiveService,
            IQueenService queenService,
            ITreatmentService treatmentService,
            IInspectionService inspectionService,
            IHarvestService harvestService,
            IQuickChartService quickChartService,
            IEmailSender emailSender,
            IConfiguration configuration,
            ISystemNotificationService systemNotificationService,
            IUserTaskService userTaskService)
        {
            this.userManager = userManager;
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
            this.queenService = queenService;
            this.treatmentService = treatmentService;
            this.inspectionService = inspectionService;
            this.harvestService = harvestService;
            this.quickChartService = quickChartService;
            this.emailSender = emailSender;
            this.configuration = configuration;
            this.systemNotificationService = systemNotificationService;
            this.userTaskService = userTaskService;
        }

        public async Task<ActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var viewModel = new IndexHomeViewModel();

            var treatmentsCount = this.treatmentService.GetAllUserTreatmentsForLastYearCount(currentUser.Id);
            var inspectionsCount = this.inspectionService.GetAllUserInspectionsForLastYearCount(currentUser.Id);
            var harvestsCount = this.harvestService.GetAllUserHarvestsForLastYearCount(currentUser.Id);

            viewModel.TreatmentsCount = treatmentsCount;
            viewModel.InspectionsCount = inspectionsCount;
            viewModel.HarvestsCount = harvestsCount;

            // TODO: Make service for home controller

            // apiaries chart
            var apiaries = this.apiaryService.GetAllUserApiaries<ApiaryDataModel>(currentUser.Id);
            viewModel.ApiariesCount = apiaries.Count();

            var apiariesCountByType = apiaries.GroupBy(a => a.ApiaryType).ToDictionary(k => k.Key, v => v.Count());
            viewModel.ApiariesCountByType = apiariesCountByType;

            var apiariesCountChartUrl = this.quickChartService.ImageUrl(
                "pie",
                apiariesCountByType.Values.ToList(),
                GlobalConstants.ApiaryTypeChartColors.Take(apiariesCountByType.Values.Count).ToArray());
            viewModel.ApiariesCountChartUrl = apiariesCountChartUrl;

            // beehives chart
            var beehives = this.beehiveService.GetAllUserBeehives<BeehiveDataModel>(currentUser.Id);
            viewModel.BeehivesCount = beehives.Count();

            var beehivesCountByPower = new Dictionary<BeehivePower, int>();
            beehivesCountByPower.Add(BeehivePower.Strong, beehives.Where(b => b.BeehivePower == BeehivePower.Strong).Count());
            beehivesCountByPower.Add(BeehivePower.Medium, beehives.Where(b => b.BeehivePower == BeehivePower.Medium).Count());
            beehivesCountByPower.Add(BeehivePower.Weak, beehives.Where(b => b.BeehivePower == BeehivePower.Weak).Count());
            viewModel.BeehivesCountByPower = beehivesCountByPower;

            var beehivesCountChartUrl = this.quickChartService.ImageUrl(
                "pie",
                beehivesCountByPower.Values.ToList(),
                GlobalConstants.BeehivePowerChartColors);
            viewModel.BeehivesCountChartUrl = beehivesCountChartUrl;

            // queens chart
            var queens = this.queenService.GetAllUserQueens<QueenDataModel>(currentUser.Id);
            viewModel.QueensCount = queens.Count();

            var queensCountByGivingDate = queens.ToList().OrderBy(q => q.GivingDate).GroupBy(q => q.GivingDate.Year).ToDictionary(k => k.Key, v => v.Count());

            var queenColors = new List<string>();

            foreach (var year in queensCountByGivingDate.Keys.OrderBy(k => k))
            {
                switch (year % 10)
                {
                    case 0:
                    case 5: queenColors.Add("'#689dd6'"); break;
                    case 1:
                    case 6: queenColors.Add("'#FFFFFF'"); break;
                    case 2:
                    case 7: queenColors.Add("'#f2c72b'"); break;
                    case 3:
                    case 8: queenColors.Add("'#E15759'"); break;
                    case 4:
                    case 9: queenColors.Add("'#59A14F'"); break;
                }
            }

            if (beehives.Count() - queens.Count() > 0)
            {
                queensCountByGivingDate.Add(0, beehives.Count() - queens.Count());
                queenColors.Add("'#d3d3d3'");
            }

            var queensCountByGivingDateChart = this.quickChartService.ImageUrl(
                "pie",
                queensCountByGivingDate.Values.ToList(),
                queenColors.ToArray(),
                "'#7B7B7B'");
            viewModel.QueenChartColors = queenColors;
            viewModel.QueensCountByGivingDate = queensCountByGivingDate;
            viewModel.QueensCountByGivingDateChartUrl = queensCountByGivingDateChart;

            viewModel.LastUserTasks = this.userTaskService.GetLastUserTasks<UserTaskViewModel>(currentUser.Id);
            viewModel.UserTasksCountByColor = new Dictionary<string, int>();
            viewModel.UserTasksCountByColor.Add("#BF3C30", this.userTaskService.GetUserTasksCountByColor(currentUser.Id, "#BF3C30"));
            viewModel.UserTasksCountByColor.Add("#F2A30F", this.userTaskService.GetUserTasksCountByColor(currentUser.Id, "#F2A30F"));
            viewModel.UserTasksCountByColor.Add("#61912A", this.userTaskService.GetUserTasksCountByColor(currentUser.Id, "#61912A"));
            viewModel.UserTasksCountByColor.Add("#639CBF", this.userTaskService.GetUserTasksCountByColor(currentUser.Id, "#639CBF"));

            return this.View(viewModel);
        }

        public IActionResult HttpError(int statusCode, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "Не успяхме да намерим стрaницата която търсите";
            }

            var viewModel = new HttpErrorViewModel
            {
                StatusCode = statusCode,
                Message = message,
            };

            return this.View(viewModel);
        }

        public IActionResult AdditionalPage()
        {
            var viewModel = new AdditionalPageViewModel
            {
                SystemNotifications = this.systemNotificationService.GetAllNotifications<SystemNotificationViewModel>(),
            };

            return this.View(viewModel);
        }
    }
}
