namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Queens;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class QueenController : Controller
    {
        private readonly IQueenService queenService;
        private readonly UserManager<ApplicationUser> userManager;

        public QueenController(IQueenService queenService,UserManager<ApplicationUser> userManager)
        {
            this.queenService = queenService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All(int id)
        {
            var currUser = await this.userManager.GetUserAsync(this.User);
            if (id == 0)
            {
                var allUserQueens = this.queenService.GetAllUserQueens<UserQueenViewModel>(currUser.Id);
                var viewModel = new AllUserQueensViewModel()
                {
                    UserQueens = allUserQueens,
                };
                return this.View(viewModel);
            }
            else
            {
                var allQueens = this.queenService.GetAllQueens<UserQueenViewModel>(id, currUser.Id);
                var viewModel = new AllUserQueensViewModel()
                {
                    UserQueens = allQueens,
                };
                return this.View(viewModel);
            }
        }

        public IActionResult Create()
        {
            return this.View();
        }

        public IActionResult ById(int id)
        {
            var queen = this.queenService.GetQueenById<QueenDataViewModel>(id);
            return this.View(queen);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateQueenInputModel inputModel)
        {
            var currUser = await this.userManager.GetUserAsync(this.User);
            var queenId = await this.queenService.CreateQueen(inputModel, 1, currUser.Id);
            return this.Redirect($"/Queen/{queenId}");
        }
    }
}
