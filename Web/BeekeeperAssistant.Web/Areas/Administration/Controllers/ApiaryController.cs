namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Administration.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using Microsoft.AspNetCore.Mvc;

    public class ApiaryController : AdministrationController
    {
        private readonly IApiaryService apiaryService;

        public ApiaryController(IApiaryService apiaryService)
        {
            this.apiaryService = apiaryService;
        }

        public IActionResult All(int page = 1)
        {
            var allCount = this.apiaryService.GetAllApiariesWithDeletedCount();
            var pagesCount = (int)Math.Ceiling((double)allCount / GlobalConstants.ApiariesPerPageAdministration);

            if (page <= 0)
            {
                page = 1;
            }
            else if (page > pagesCount)
            {
                page = pagesCount == 0 ? 1 : pagesCount;
            }

            var viewModel = new AllApiariesAdministrationViewModel
            {
                AllApiaries = this.apiaryService.GetAllApiariesWithDeleted<ApiaryViewModel>(
                    GlobalConstants.ApiariesPerPageAdministration,
                    (page - 1) * GlobalConstants.ApiariesPerPageAdministration),
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
        public async Task<IActionResult> Undelete(int id, string returnUrl)
        {
            await this.apiaryService.UndeleteAsync(id);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно възстановен пчелин!";

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl);
            }
            return this.RedirectToAction(nameof(this.All));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, string returnUrl)
        {
            await this.apiaryService.DeleteApiaryByIdAsync(id);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно изтрит пчелин!";

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
