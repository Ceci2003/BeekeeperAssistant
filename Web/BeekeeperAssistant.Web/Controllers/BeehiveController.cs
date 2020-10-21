namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [Authorize]
    public class BeehiveController : BaseController
    {
        private readonly IDeletableEntityRepository<Beehive> beehiveRepository;
        private readonly IBeehiveService beehiveService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<Apiary> apiaryRepository;

        public BeehiveController(
            IDeletableEntityRepository<Beehive> beehiveRepository,
            IBeehiveService beehiveService,
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<Apiary> apiaryRepository)
        {
            this.beehiveRepository = beehiveRepository;
            this.beehiveService = beehiveService;
            this.userManager = userManager;
            this.apiaryRepository = apiaryRepository;
        }

        // Does not work!
        public IActionResult GetByNumber()
        {
            return this.View();
        }

        // Add Action Create
        public async Task<IActionResult> Create(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var currentApiary = this.apiaryRepository.All().Where(b => b.Id == id).FirstOrDefault();
            if (currentApiary.CreatorId != currentUser.Id)
            {
                return this.Forbid();
            }

            this.ViewData["ApiaryId"] = id;
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBeehiveInputModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            // Check if there is already a beehive with the same Nuumber
            if (this.beehiveService.NumberExists(inputModel.Number, user))
            {
                this.ModelState.AddModelError("Number", "Number already exists");
                return this.View(inputModel);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var beehive = new Beehive()
            {
                ApiaryId = inputModel.ApiaryId,
                BeehivePower = inputModel.BeehivePower,
                BeehiveSystem = inputModel.BeehiveSystem,
                BeehiveType = inputModel.BeehiveType,
                Date = inputModel.Date,
                Number = inputModel.Number,
                HasDevice = inputModel.HasDevice,
                HasPolenCatcher = inputModel.HasPolenCatcher,
                HasPropolisCatcher = inputModel.HasPropolisCatcher,
            };

            await this.beehiveRepository.AddAsync(beehive);
            await this.beehiveRepository.SaveChangesAsync();

            return this.Redirect("/");
        }

        // Add Action All
    }
}
