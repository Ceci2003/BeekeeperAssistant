namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Administration.ApiaryHelpers;
    using Microsoft.AspNetCore.Mvc;

    public class ApiaryHelperController : AdministrationController
    {
        private readonly IApiaryHelperService apiaryHelperService;

        public ApiaryHelperController(IApiaryHelperService apiaryHelperService)
        {
            this.apiaryHelperService = apiaryHelperService;
        }

        public IActionResult All(int page = 1)
        {
            var allCount = this.apiaryHelperService.GetAllApiaryHelpersWithDeletedCount();
            var pagesCount = (int)Math.Ceiling((double)allCount / GlobalConstants.ApiaryHelpersPerPageAdministration);

            if (page <= 0)
            {
                page = 1;
            }
            else if (page > pagesCount)
            {
                page = pagesCount == 0 ? 1 : pagesCount;
            }

            var viewModel = new AllApiaryHelpersAdministrationViewModel
            {
                AllApiariesHelpers = this.apiaryHelperService.GetAllApiaryHelpers<ApiaryHelperAdministrationViewModel>(
                    GlobalConstants.ApiaryHelpersPerPageAdministration,
                    (page - 1) * GlobalConstants.ApiaryHelpersPerPageAdministration),
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
        public async Task<IActionResult> Delete(string userId, int apiaryId, string returnUrl)
        {
            await this.apiaryHelperService.DeleteAsync(userId, apiaryId);

            this.TempData[GlobalConstants.SuccessMessage] = "Успешно изтрит помощник!";

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl);
            }

            return this.Redirect($"/ApiaryHelper/All/{apiaryId}");
        }
    }
}
