namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Administration.Dashboard;

    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class DashboardController : AdministrationController
    {
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;
        private readonly IApiaryHelperService apiaryHelperService;
        private readonly IUserService userService;

        public DashboardController(
            IApiaryService apiaryService,
            IBeehiveService beehiveService,
            IApiaryHelperService apiaryHelperService,
            IUserService userService)
        {
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
            this.apiaryHelperService = apiaryHelperService;
            this.userService = userService;
        }

        public async Task<ActionResult> Index()
        {
            var viewModel = new AdministrationIndexDashboardViewModel()
            {
                AllUsersCount = this.userService.AllUsersCount(),
                AllHelpersCount = this.apiaryHelperService.GetAllApiaryHelpersCount(),
                AllAdministratorsCount = await this.userService.AllAdministratorsCountAsync(),
                AllApiariesCount = this.apiaryService.AllApiariesCount(),
                AllBeehivesCount = this.beehiveService.AllBeehivesCount(),
            };

            return this.View(viewModel);
        }
    }
}
