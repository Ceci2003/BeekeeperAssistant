namespace BeekeeperAssistant.Web.ViewComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using Microsoft.AspNetCore.Mvc;

    public class ApiaryInfoViewComponent : ViewComponent
    {
        private readonly IApiaryService apiaryService;

        public ApiaryInfoViewComponent(IApiaryService apiaryService)
        {
            this.apiaryService = apiaryService;
        }

        public IViewComponentResult Invoke(int apiId)
        {
            var viewModel = this.apiaryService.GetApiaryById<ApiaryDataViewModel>(apiId);
            return this.View(viewModel);
        }
    }
}
