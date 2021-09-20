namespace BeekeeperAssistant.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;
        private readonly ITreatmentService treatmentService;
        private readonly IInspectionService inspectionService;
        private readonly IHarvestService harvestService;
        private readonly IQuickChartService quickChartService;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IApiaryService apiaryService,
            IBeehiveService beehiveService,
            ITreatmentService treatmentService,
            IInspectionService inspectionService,
            IHarvestService harvestService,
            IQuickChartService quickChartService)
        {
            this.userManager = userManager;
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
            this.treatmentService = treatmentService;
            this.inspectionService = inspectionService;
            this.harvestService = harvestService;
            this.quickChartService = quickChartService;
        }

        public async Task<ActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.User.Identity.IsAuthenticated)
            {
                return this.View();
            }

            var viewModel = new IndexViewModel();

            var treatmentsCount = this.treatmentService.GetAllUserTreatmentsForLastYearCount(currentUser.Id);
            var inspectionsCount = this.inspectionService.GetAllUserInspectionsForLastYearCount(currentUser.Id);
            var harvestsCount = this.harvestService.GetAllUserHarvestsForLastYearCount(currentUser.Id);

            viewModel.TreatmentsCount = treatmentsCount;
            viewModel.InspectionsCount = inspectionsCount;
            viewModel.HarvestsCount = harvestsCount;

            var apiaries = this.apiaryService.GetAllUserApiaries<ApiaryViewModel>(currentUser.Id);

            viewModel.ApiariesCount = apiaries.Count();

            var apiariesCountByType = apiaries.ToList().GroupBy(a => a.ApiaryType).ToDictionary(k => k.Key, v => v.ToList().Count);
            viewModel.ApiariesCountByType = apiariesCountByType;

            var apiariesCountChartUrl = this.quickChartService.ImageUrl(
                "pie",
                apiariesCountByType.Values.ToList(),
                GlobalConstants.ApiaryChartColors.Take(apiariesCountByType.Values.Count).ToArray());
            viewModel.ApiariesCountChartUrl = apiariesCountChartUrl;

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult About()
        {
            return this.View();
        }

        public IActionResult Contact()
        {
            return this.View();
        }

        public IActionResult HttpError(int statusCode, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = "Не успяхме да намерим стряницата която търсите";
            }

            var viewModel = new HttpErrorViewModel
            {
                StatusCode = statusCode,
                Message = message,
            };

            return this.View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
            //return this.HttpError(404, string.Empty);
        }
    }
}
