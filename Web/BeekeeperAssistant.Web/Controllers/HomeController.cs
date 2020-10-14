namespace BeekeeperAssistant.Web.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IApiaryService apiaryService;

        public HomeController(IApiaryService apiaryService)
        {
            this.apiaryService = apiaryService;
        }

        [Route("{*url}", Order = 999)]
        public IActionResult HttpError()
        {
            this.HttpContext.Response.StatusCode = 404;
            return this.View();

            // TODO: This may be done better with filters. If you return this.NotFound() it won't show HttpError page!
        }

        public IActionResult Index()
        {
            return this.View();
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
