namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

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

        public IActionResult All(int page = 1)
        {
            if (page <= 0)
            {
                page = 1;
            }

            var allCount = this.userService.GetAllUsersWithDeletedCount();
            var pagesCount = (int)Math.Ceiling((double)allCount / GlobalConstants.UsersPerPageAdministration);

            var viewModel = new AdministrationAllUserViewModel
            {
                AllUsers = this.userService.GetAllUsersWithDeleted<UserViewModel>(
                GlobalConstants.UsersPerPageAdministration,
                (page - 1) * GlobalConstants.UsersPerPageAdministration),
                PagesCount = pagesCount,
            };

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = page;

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

        public IActionResult AllAdministrators(int page = 1)
        {
            var allCount = this.userService.GetAllUsersInRoleWithDeletedCount(GlobalConstants.AdministratorRoleName);
            var pagesCount = (int)Math.Ceiling((double)allCount / GlobalConstants.UsersPerPageAdministration);

            if (page <= 0)
            {
                page = 1;
            }
            else if (page > pagesCount)
            {
                page = pagesCount == 0 ? 1 : pagesCount;
            }

            var viewModel = new AdministrationAllUserViewModel
            {
                AllUsers = this.userService.GetAllUsersInRoleWithDeleted<UserViewModel>(
                GlobalConstants.AdministratorRoleName,
                GlobalConstants.UsersPerPageAdministration,
                (page - 1) * GlobalConstants.UsersPerPageAdministration),
                PagesCount = pagesCount,
            };

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            viewModel.CurrentPage = page;

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
