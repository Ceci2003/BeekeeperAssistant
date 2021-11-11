namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

        public IActionResult All()
        {
            var viewModel = new AllApiariesAdministrationViewModel
            {
                AllApiaries = this.apiaryService.GetAllApiariesWithDeleted<ApiaryViewModel>(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Undelete(int id)
        {
            await this.apiaryService.UndeleteAsync(id);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
