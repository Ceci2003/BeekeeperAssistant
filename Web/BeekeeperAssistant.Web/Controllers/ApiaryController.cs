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

    [Authorize]
    public class ApiaryController : BaseController
    {
        private readonly IApiaryService apiaryService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<UsersApiaries> userApiRepository;
        private readonly IDeletableEntityRepository<Apiary> apiaryRepository;
        private readonly IBeehiveService beehiveService;

        public ApiaryController(
            IApiaryService apiaryService,
            UserManager<ApplicationUser> userManager,
            IRepository<UsersApiaries> userApiRepository,
            IDeletableEntityRepository<Apiary> apiaryRepository,
            IBeehiveService beehiveService)
        {
            this.apiaryService = apiaryService;
            this.userManager = userManager;
            this.userApiRepository = userApiRepository;
            this.apiaryRepository = apiaryRepository;
            this.beehiveService = beehiveService;
        }

        public async Task<IActionResult> GetByNumber(string apiNumber)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var apiary = this.apiaryService.GetApiaryByNumber(apiNumber, currentUser);

            if (apiary == null)
            {
                return this.Forbid();
            }

            var allApiaryBeehives = this.beehiveService.GetAllUserBeehivesByApiaryId<UserBeehiveViewModel>(apiary.Id);

            var viewModel = new AllUserBeehivesViewModel()
            {
                AllUserBeehives = allApiaryBeehives,
            };
            return this.View(viewModel);
        }

        // Make shure everything is clean!
        // Make Seeding!
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await this.apiaryService.DeleteById(id);
            return this.Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateApiaryInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            if (this.apiaryService.ApiaryExists(inputModel.Number, currentUser))
            {
                this.ModelState.AddModelError("Number", "Invalid apiary number!");
                return this.View(inputModel);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.apiaryService.AddApiary(currentUser, inputModel);

            return this.Redirect("/");
        }

        public async Task<IActionResult> All()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var allApiarires = this.apiaryService.GetAllUserApiaries<UserApiaryViewModel>(currentUser.Id);
            var viewModel = new AllUserApiariesViewModel()
            {
                AllUserApiaries = allApiarires,
            };
            return this.View(viewModel);
        }
    }
}
