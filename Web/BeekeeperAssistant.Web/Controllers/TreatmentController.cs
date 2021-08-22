namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class TreatmentController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITreatmentService treatmentService;

        public TreatmentController(
            UserManager<ApplicationUser> userManager,
            ITreatmentService treatmentService)
        {
            this.userManager = userManager;
            this.treatmentService = treatmentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string data)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var id = await this.treatmentService.CreateTreatment(
                currentUser.Id,
                DateTime.Now,
                "test",
                "note",
                "disease",
                "medicament",
                InputAs.PerHive,
                3,
                Dose.Strips,
                new List<int> { 1 });

            return this.View(id);
        }
    }
}
