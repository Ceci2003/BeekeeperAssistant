namespace BeekeeperAssistant.Web.Controllers
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

    public class ApiaryNoteController : BaseController
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

        public async Task<IActionResult> AllByApiaryId(int apiaryId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var viewModel = new AllByApiaryIdApiaryNoteViewModel();

            viewModel.AllNotes = this.apiaryNoteService.GetAllApiaryNotes<ByApiaryIdApiaryNoteViewModel>(apiaryId);
            viewModel.ApiaryAccess = await this.apiaryHelperService.GetUserApiaryAccessAsync(currentUser.Id, apiaryId);

            var apiary = this.apiaryService.GetApiaryById<ApiaryDataModel>(apiaryId);
            viewModel.ApiaryId = apiaryId;
            viewModel.ApiaryNumber = apiary.Number;
            viewModel.ApiaryName = apiary.Name;

            return this.View(viewModel);
        }

        public IActionResult Create(int apiaryId)
        {
            var viewModel = new CreateApiaryNoteInputModel();

            var apiary = this.apiaryService.GetApiaryById<ApiaryDataModel>(apiaryId);
            viewModel.ApiaryId = apiaryId;
            viewModel.ApiaryNumber = apiary.Number;
            viewModel.ApiaryName = apiary.Name;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int apiaryId, CreateApiaryNoteInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.apiaryNoteService.CreateAsync(
                    apiaryId,
                    inputModel.Title,
                    inputModel.Content,
                    inputModel.Color,
                    currentUser.Id);

            return this.RedirectToAction("AllByApiaryId", "ApiaryNote", new { apiaryId = apiaryId });
        }

        public IActionResult Edit(int noteId)
        {
            var viewModel = this.apiaryNoteService.GetApiaryNoteById<EditApiaryNoteInputModel>(noteId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int noteId, CreateApiaryNoteInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var apiaryId = await this.apiaryNoteService.EditAsync(
                noteId,
                inputModel.Title,
                inputModel.Content,
                inputModel.Color,
                currentUser.Id);

            return this.RedirectToAction("AllByApiaryId", "ApiaryNote", new { apiaryId = apiaryId });
        }

        public async Task<IActionResult> Delete(int noteId)
        {
            var apiaryId = await this.apiaryNoteService.DeleteAsync(noteId);

            return this.RedirectToAction("AllByApiaryId", "ApiaryNote", new { apiaryId = apiaryId });
        }
    }
}
