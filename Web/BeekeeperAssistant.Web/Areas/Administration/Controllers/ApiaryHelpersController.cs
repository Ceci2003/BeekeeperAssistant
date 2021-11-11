namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Administration.ApiaryHelpers;
    using Microsoft.AspNetCore.Mvc;

    public class ApiaryHelpersController : AdministrationController
    {
        private readonly IApiaryHelperService apiaryHelperService;

        public ApiaryHelpersController(IApiaryHelperService apiaryHelperService)
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
    }
}
