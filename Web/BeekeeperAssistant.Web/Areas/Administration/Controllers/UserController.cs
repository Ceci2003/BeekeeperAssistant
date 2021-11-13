namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class UserController : AdministrationController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;

        public UserController(
            UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            this.userManager = userManager;
            this.userService = userService;
        }

        public IActionResult All()
        {
            var viewModel = new AllUsersViewModel
            {
                AllUsers = this.userService.GetAllUsersWithDeleted<UserViewModel>(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await this.userManager
                .FindByIdAsync(id);

            if (user == null)
            {
                return this.NotFound();
            }

            await this.userService.DeleteAsync(user);

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно изтрит потребител!";

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult AllAdministrators()
        {
            var viewModel = new AllUsersViewModel
            {
                AllUsers = this.userService.GetAllUsersInRole<UserViewModel>(GlobalConstants.AdministratorRoleName),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Undelete(string id)
        {
            var user = this.userService.GetUserByIdWithUndeleted(id);

            await this.userService.UndeleteAsync(user);

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно възстановен потребител!";
            return this.RedirectToAction(nameof(this.All));
        }
    }
}
