namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.EntityFrameworkCore.Query.Internal;

    [Authorize]
    public class BeehiveController : BaseController
    {
        private readonly IDeletableEntityRepository<Beehive> beehiveRepository;
        private readonly IBeehiveService beehiveService;
        private readonly IApiaryService apiaryService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<Apiary> apiaryRepository;

        public BeehiveController(
            IDeletableEntityRepository<Beehive> beehiveRepository,
            IBeehiveService beehiveService,
            IApiaryService apiaryService,
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<Apiary> apiaryRepository)
        {
            this.beehiveRepository = beehiveRepository;
            this.beehiveService = beehiveService;
            this.apiaryService = apiaryService;
            this.userManager = userManager;
            this.apiaryRepository = apiaryRepository;
        }

        // Does not work!
        public async Task<IActionResult> GetById(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var beehive = this.beehiveService.GetBeehiveById(id);
            beehive.Apiary = this.apiaryRepository.All().Where(a => a.Id == beehive.ApiaryId).FirstOrDefault();

            if (beehive == null)
            {
                return this.Forbid();
            }

            var viewModel = new BeehiveDataViewModel()
            {
                Id = beehive.Id,
                Number = beehive.Number,
                BeehiveSystem = beehive.BeehiveSystem,
                BeehiveType = beehive.BeehiveType,
                CreationDate = beehive.Date,
                BeehivePower = beehive.BeehivePower,
                ApiId = beehive.ApiaryId,
                Apiary = beehive.Apiary,
                ApiaryNumber = beehive.Apiary.Number,
                HasDevice = beehive.HasDevice == true ? true : false,
                HasPolenCatcher = beehive.HasPolenCatcher == true ? true : false,
                HasPropolisCatcher = beehive.HasPropolisCatcher == true ? true : false,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Create(int id)
        {
            var inputModel = new CreateBeehiveInputModel();

            if (id == 0)
            {
                var currentUser = await this.userManager.GetUserAsync(this.User);
                var allUserApiariries = this.apiaryService.GetAllUserApiaries<SelectListOptionApiaryViewModel>(currentUser.Id);
                inputModel.AllApiaries = allUserApiariries;
            }

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id, CreateBeehiveInputModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var apiId = id == 0 ? inputModel.ApiId : id;

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            // Check if there is already a beehive with the same Nuumber
            if (this.beehiveService.NumberExists(inputModel.Number, user))
            {
                this.ModelState.AddModelError("Number", "Number already exists");
                return this.View(inputModel);
            }

            var api = this.apiaryService.GetApiaryById<UserApiaryViewModel>(apiId);
            if (api.CreatorId != user.Id)
            {
                return this.BadRequest();
            }

            await this.beehiveService.AddUserBeehive(user, inputModel, apiId);

            return this.Redirect($"/Apiary/{api.Number}");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var beehive = this.beehiveService.GetUserBeehiveById<EditBeehiveInputModel>(id, currentUser);
            var allUserApiariries = this.apiaryService.GetAllUserApiaries<SelectListOptionApiaryViewModel>(currentUser.Id);
            beehive.AllApiaries = allUserApiariries;

            if (beehive?.CreatorId != currentUser?.Id)
            {
                return this.Forbid();
            }

            return this.View(beehive);
        }

        public async Task<IActionResult> All()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var allUserBeehives = this.beehiveService.GetAllUserBeehives<UserBeehiveViewModel>(currentUser);
            var viewModel = new AllUserBeehivesViewModel()
            {
                AllUserBeehives = allUserBeehives,
            };
            return this.View(viewModel);
        }

    }
}
