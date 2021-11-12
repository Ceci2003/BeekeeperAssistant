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

        public IActionResult All()
        {
            var viewModel = new AllApiaryHelpersAdministrationViewModel
            {
                AllApiariesHelpers = this.apiaryHelperService.GetAllApiaryHelpers<ApiaryHelperAdministrationViewModel>(),
            };

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
