namespace BeekeeperAssistant.Web.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IApiaryService apiaryService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(
            IApiaryService apiaryService,
            UserManager<ApplicationUser> userManager)
        {
            this.apiaryService = apiaryService;
            this.userManager = userManager;
        }

        [Route("{*url}", Order = 999)]
        public IActionResult HttpError()
        {
            this.HttpContext.Response.StatusCode = 404;
            return this.View();

            // TODO: This may be done better with filters. If you return this.NotFound() it won't show HttpError page!
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            if (currentUser == null)
            {
                return this.View();
            }
            else
            {
                var viewModel = new LoginHomeViewModel()
                {
                    Count = this.apiaryService.GetAllUserApiaries<UserApiaryViewModel>(currentUser?.Id).ToList().Count,
                };

                return this.View(viewModel);
            }
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
