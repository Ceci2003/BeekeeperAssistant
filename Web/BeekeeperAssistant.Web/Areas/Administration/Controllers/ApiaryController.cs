namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Filters.Models;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Administration.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using Microsoft.AspNetCore.Mvc;

    public class ApiaryController : AdministrationController
    {
        private readonly IApiaryService apiaryService;
        private readonly ITypeService typeService;

        public ApiaryController(
            IApiaryService apiaryService,
            ITypeService typeService)
        {
            this.apiaryService = apiaryService;
            this.typeService = typeService;
        }

        public IActionResult All(
            FilterModel filterModel,
            int page = 1)
        {
            if (page <= 0)
            {
                page = 1;
            }

            var allCount = this.apiaryService.GetAllApiariesWithDeletedCount();
            var pagesCount = (int)Math.Ceiling((double)allCount / GlobalConstants.ApiariesPerPageAdministration);

            var viewModel = new AdministrationAllApiaryViewModel
            {
                ApiariesFilter = new FilterModel
                {
                    Data = new FilterData
                    {
                        ModelProperties = this.typeService.GetAllTypePropertiesName(typeof(AdministrationAllApiaryFilterModel)),
                        ModelPropertiesDisplayNames = this.typeService.GetAllTypePropertiesDisplayName(typeof(AdministrationAllApiaryFilterModel)),
                        PageNumber = page,
                    },
                },
                AllApiaries = this.apiaryService.GetAllApiariesWithDeleted<AdministrationAllApiaryAllApiariesViewModel>(
                    GlobalConstants.ApiariesPerPageAdministration,
                    (page - 1) * GlobalConstants.ApiariesPerPageAdministration,
                    filterModel),
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
