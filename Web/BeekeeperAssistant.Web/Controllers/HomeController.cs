﻿namespace BeekeeperAssistant.Web.Controllers
{
    using System.Diagnostics;
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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApiaryService apiaryService;
        private readonly IBeehiveService beehiveService;
        private readonly ITreatmentService treatmentService;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IApiaryService apiaryService,
            IBeehiveService beehiveService,
            ITreatmentService treatmentService)
        {
            this.userManager = userManager;
            this.apiaryService = apiaryService;
            this.beehiveService = beehiveService;
            this.treatmentService = treatmentService;
        }

        public async Task<IActionResult> Index()
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
