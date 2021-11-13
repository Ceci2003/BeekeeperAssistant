using BeekeeperAssistant.Common;
using BeekeeperAssistant.Services.Data;
using BeekeeperAssistant.Web.ViewModels.Administration.Beehives;
using BeekeeperAssistant.Web.ViewModels.Beehives;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    public class BeehiveController : AdministrationController
    {
        private readonly IBeehiveService beehiveService;
        private readonly IApiaryService apiaryService;

        public BeehiveController(IBeehiveService beehiveService, IApiaryService apiaryService)
        {
            this.beehiveService = beehiveService;
            this.apiaryService = apiaryService;
        }

        public IActionResult All()
        {
            var viewModel = new AllBeehivesAdministrationViewModel
            {
                AllBeehives = this.beehiveService.GetAllBeehivesWithDeleted<BeehivesAdministrationViewModel>(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Undelete(int id, string returnUrl)
        {
            await this.beehiveService.UndeleteAsync(id);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно възстановен кошер!";

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, string returnUrl)
        {
            await this.beehiveService.DeleteBeehiveByIdAsync(id);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно изтрит кошер!";

            if (returnUrl != null)
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
