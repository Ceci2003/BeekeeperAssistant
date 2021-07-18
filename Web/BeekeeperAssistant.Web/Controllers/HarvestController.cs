namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using BeekeeperAssistant.Web.ViewModels.Harvest;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HarvestController : BaseController
    {
        private readonly IHarvestService harvestService;
        private readonly IBeehiveService beehiveService;
        private readonly UserManager<ApplicationUser> userManager;

        public HarvestController(
            IHarvestService harvestService,
            IBeehiveService beehiveService,
            UserManager<ApplicationUser> userManager)
        {
            this.harvestService = harvestService;
            this.beehiveService = beehiveService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = new AllHarvestsViewModel
            {
                AllHarvests = this.harvestService.GetAllUserHarvests<HarvestDatavVewModel>(user.Id),
            };

            return this.View(viewModel);
        }

        public IActionResult ById(int id)
        {
            var viewModel = this.harvestService.GetHarvestById<HarvestDatavVewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        public IActionResult Create(int id)
        {
            var inputModel = new CreateHarvestInputModel
            {
                BeehiveId = id,
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateHarvestInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var harvestId = await this.harvestService.
                CreateUserHarvestAsync(
                    user.Id,
                    inputModel.BeehiveId,
                    inputModel.HarvestName,
                    inputModel.DateOfHarves,
                    inputModel.Product,
                    inputModel.HoneyType,
                    inputModel.Note,
                    inputModel.Amount);

            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataViewModel>(inputModel.BeehiveId);

            return this.Redirect($"/Beehive/{beehive.ApiaryNumber}/{beehive.Id}");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var inputModel = this.harvestService.GetHarvestById<EditHarvestInputModel>(id);
            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataViewModel>(inputModel.BeehiveId);

            await this.harvestService.DeleteHarvestAsync(id);

            return this.Redirect($"/Beehive/{beehive.ApiaryNumber}/{beehive.Id}");
        }

        public IActionResult Edit(int id)
        {
            var inputModel = this.harvestService.GetHarvestById<EditHarvestInputModel>(id);
            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditHarvestInputModel inputModel)
        {
            var harvestId = await this.harvestService.EditHarvestAsync(
                id,
                inputModel.HarvestName,
                inputModel.DateOfHarves,
                inputModel.Product,
                inputModel.HoneyType,
                inputModel.Note,
                inputModel.Amount);

            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataViewModel>(inputModel.BeehiveId);

            return this.Redirect($"/Beehive/{beehive.ApiaryNumber}/{beehive.Id}");
        }
    }
}
