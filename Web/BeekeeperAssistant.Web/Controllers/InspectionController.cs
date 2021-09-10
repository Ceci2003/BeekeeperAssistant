namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using BeekeeperAssistant.Web.ViewModels.Inspection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class InspectionController : BaseController
    {
        private readonly IInspectionService inspectionService;
        private readonly IBeehiveService beehiveService;
        private readonly IApiaryService apiaryService;
        private readonly UserManager<ApplicationUser> userManager;

        public InspectionController(
            IInspectionService inspectionService,
            IBeehiveService beehiveService,
            IApiaryService apiaryService,
            UserManager<ApplicationUser> userManager)
        {
            this.inspectionService = inspectionService;
            this.beehiveService = beehiveService;
            this.apiaryService = apiaryService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Create(int? id)
        {
            var currentuser = await this.userManager.GetUserAsync(this.User);

            var inputModel = new CreateInspectionInputModel
            {
                DateOfInspection = DateTime.UtcNow.Date,
            };

            if (id == null)
            {
                inputModel.Apiaries = this.apiaryService.GetUserApiariesAsKeyValuePairs(currentuser.Id);
            }
            else
            {
                inputModel.ApiaryId = this.apiaryService.GetApiaryIdByBeehiveId(id.Value);
            }

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, CreateInspectionInputModel inputModel)
        {
            var currentuser = await this.userManager.GetUserAsync(this.User);

            if (id == null)
            {
                var apiary = this.apiaryService.GetApiaryById<ApiaryViewModel>(inputModel.ApiaryId);
                var beehiveNumber = int.Parse(inputModel.SelectedBeehiveNumber.ToString());
                var beehive = this.beehiveService.GetBeehiveByNumber<BeehiveViewModel>(beehiveNumber, apiary.Number);

                await this.inspectionService.CreateUserInspectionAsync(currentuser.Id, beehive.Id, inputModel);

                return this.RedirectToAction("ById", "Beehive", new { beehiveId = beehive.Id, tabPage = "Inspections" });
            }
            else
            {
                await this.inspectionService.CreateUserInspectionAsync(currentuser.Id, id.Value, inputModel);

                return this.RedirectToAction("ById", "Beehive", new { beehiveId = id.Value, tabPage = "Inspections" });
            }
        }
    }
}
