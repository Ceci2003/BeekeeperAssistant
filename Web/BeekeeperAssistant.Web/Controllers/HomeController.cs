namespace BeekeeperAssistant.Web.Controllers
{
    using System.Diagnostics;

    using BeekeeperAssistant.Web.ViewModels;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
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

        public IActionResult HttpError(int statusCode)
        {
            var viewModel = new HttpErrorViewModel
            {
                StatusCode = statusCode,
            };

            return this.View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
