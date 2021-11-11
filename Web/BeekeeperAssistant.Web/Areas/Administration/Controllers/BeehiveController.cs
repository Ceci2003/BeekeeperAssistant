using BeekeeperAssistant.Services.Data;
using BeekeeperAssistant.Web.ViewModels.Administration.Beehives;
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

        public BeehiveController(IBeehiveService beehiveService)
        {
            this.beehiveService = beehiveService;
        }

        public IActionResult All()
        {
            var viewModel = new AllBeehivesAdministrationViewModel
            {
                AllBeehives = this.beehiveService.GetAllBeehivesWithDeleted<BeehivesAdministrationViewModel>(),
            };

            return this.View(viewModel);
        }
    }
}
