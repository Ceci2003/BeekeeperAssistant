namespace BeekeeperAssistant.Web.Controllers
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
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    public class HomeController : BaseController
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
            IConfiguration configuration)
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
        }

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

        [HttpPost]
        public async Task<IActionResult> Contact(ContactHomeInputModel inputModel)
        {
            await this.emailSender.SendEmailAsync(
                  inputModel.Email,
                  string.Empty,
                  this.configuration["SendGrid:RecipientEmail"],
                  inputModel.Subject,
                  inputModel.Content);

            return this.RedirectToAction("Index", "Home");
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}