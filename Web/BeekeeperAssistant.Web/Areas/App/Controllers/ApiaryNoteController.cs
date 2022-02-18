namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using BeekeeperAssistant.Web.ViewModels.ApiaryNotes;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ApiaryNoteController : AppBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IApiaryNoteService apiaryNoteService;
        private readonly IApiaryHelperService apiaryHelperService;
        private readonly IApiaryService apiaryService;

        public ApiaryNoteController(
            UserManager<ApplicationUser> userManager,
            IApiaryNoteService apiaryNoteService,
            IApiaryHelperService apiaryHelperService,
            IApiaryService apiaryService)
        {
            this.userManager = userManager;
            this.apiaryNoteService = apiaryNoteService;
            this.apiaryHelperService = apiaryHelperService;
            this.apiaryService = apiaryService;
        }

        public async Task<IActionResult> AllByApiaryId(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var viewModel = new AllByApiaryIdApiaryNoteViewModel();

            viewModel.AllNotes = this.apiaryNoteService.GetAllApiaryNotes<ByApiaryIdApiaryNoteViewModel>(id);
            viewModel.ApiaryAccess = await this.apiaryHelperService.GetUserApiaryAccessAsync(currentUser.Id, id);

            var apiary = this.apiaryService.GetApiaryById<ApiaryDataModel>(id);
            viewModel.ApiaryId = id;
            viewModel.ApiaryNumber = apiary.Number;
            viewModel.ApiaryName = apiary.Name;
            viewModel.ApiaryType = apiary.ApiaryType;

            this.TempData.Keep();

            return this.View(viewModel);
        }

        public IActionResult Create(int id)
        {
            var viewModel = new CreateApiaryNoteInputModel();

            var apiary = this.apiaryService.GetApiaryById<ApiaryDataModel>(id);
            viewModel.ApiaryId = id;
            viewModel.ApiaryNumber = apiary.Number;
            viewModel.ApiaryName = apiary.Name;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id, CreateApiaryNoteInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.apiaryNoteService.CreateAsync(
                    id,
                    inputModel.Title,
                    inputModel.Content,
                    inputModel.Color,
                    currentUser.Id);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно добавихте бележка на пчелина!";

            return this.RedirectToAction("AllByApiaryId", "ApiaryNote", new { id = id });
        }

        public IActionResult Edit(int id)
        {
            var viewModel = this.apiaryNoteService.GetApiaryNoteById<EditApiaryNoteInputModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateApiaryNoteInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var apiaryId = await this.apiaryNoteService.EditAsync(
                id,
                inputModel.Title,
                inputModel.Content,
                inputModel.Color,
                currentUser.Id);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно редактирахте бележката бележка на пчелина!";

            return this.RedirectToAction("AllByApiaryId", "ApiaryNote", new { id = apiaryId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var apiaryId = await this.apiaryNoteService.DeleteAsync(id);

            this.TempData[GlobalConstants.SuccessMessage] = $"Успешно изтрихте бележката бележка на пчелина!";

            return this.RedirectToAction("AllByApiaryId", "ApiaryNote", new { id = apiaryId });
        }
    }
}
