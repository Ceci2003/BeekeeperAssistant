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
            var apiId = this.apiaryService.GetApiaryIdByNumber(apiNumber, currentUser);
            var allApiaryBeehives = this.beehiveService.GetAllUserBeehivesByApiaryId<UserBeehiveViewModel>(apiId);

            var viewModel = new AllUserBeehivesViewModel()
            {
                AllUserBeehives = allApiaryBeehives,
            };
            return this.View(viewModel);
        }

        // TODO: Require Api number to be unique. Make CRUD operations in separate Service.
        // Make model validations. Make shure everything is clean!
        // Make Seeding!
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateApiaryInputModel inputModel)
        {
            var apiary = new Apiary()
            {
                Adress = inputModel.Adress,
                Name = inputModel.Name,
                Number = inputModel.Number,
                ApiaryType = inputModel.ApiaryType,
            };
            await this.apiaryRepository.AddAsync(apiary);
            await this.apiaryRepository.SaveChangesAsync();

            // Check if there is already a apiary with the same Number
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var userApiaries = new UsersApiaries()
            {
                ApiaryId = apiary.Id,
                UserId = currentUser.Id,
            };

            await this.userApiRepository.AddAsync(userApiaries);
            await this.userApiRepository.SaveChangesAsync();

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
