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
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ApiaryController : Controller
    {
        private readonly IApiaryService apiaryService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<UsersApiaries> userApiRepository;
        private readonly IDeletableEntityRepository<Apiary> apiaryRepository;

        public ApiaryController(
            IApiaryService apiaryService,
            UserManager<ApplicationUser> userManager,
            IRepository<UsersApiaries> userApiRepository,
            IDeletableEntityRepository<Apiary> apiaryRepository)
        {
            this.apiaryService = apiaryService;
            this.userManager = userManager;
            this.userApiRepository = userApiRepository;
            this.apiaryRepository = apiaryRepository;
        }

        public IActionResult GetByNumber(string apiNumber)
        {
            return this.View();
        }

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

    }
}
