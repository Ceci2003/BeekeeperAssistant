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
            var currentUser = await userManager.GetUserAsync(User);
            var viewModel = new AllByApiaryIdApiaryNoteViewModel();

            viewModel.AllNotes = apiaryNoteService.GetAllApiaryNotes<ByApiaryIdApiaryNoteViewModel>(id);
            viewModel.ApiaryAccess = await apiaryHelperService.GetUserApiaryAccessAsync(currentUser.Id, id);

            var apiary = apiaryService.GetApiaryById<ApiaryDataModel>(id);
            viewModel.ApiaryId = id;
            viewModel.ApiaryNumber = apiary.Number;
            viewModel.ApiaryName = apiary.Name;

            return View(viewModel);
        }

        public IActionResult Create(int id)
        {
            var viewModel = new CreateApiaryNoteInputModel();

            var apiary = apiaryService.GetApiaryById<ApiaryDataModel>(id);
            viewModel.ApiaryId = id;
            viewModel.ApiaryNumber = apiary.Number;
            viewModel.ApiaryName = apiary.Name;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id, CreateApiaryNoteInputModel inputModel)
        {
            var currentUser = await userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            await apiaryNoteService.CreateAsync(
                    id,
                    inputModel.Title,
                    inputModel.Content,
                    inputModel.Color,
                    currentUser.Id);

            return RedirectToAction("AllByApiaryId", "ApiaryNote", new { apiaryId = id });
        }

        public IActionResult Edit(int id)
        {
            var viewModel = apiaryNoteService.GetApiaryNoteById<EditApiaryNoteInputModel>(id);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateApiaryNoteInputModel inputModel)
        {
            var currentUser = await userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            var apiaryId = await apiaryNoteService.EditAsync(
                id,
                inputModel.Title,
                inputModel.Content,
                inputModel.Color,
                currentUser.Id);

            return RedirectToAction("AllByApiaryId", "ApiaryNote", new { apiaryId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var apiaryId = await apiaryNoteService.DeleteAsync(id);

            return RedirectToAction("AllByApiaryId", "ApiaryNote", new { apiaryId });
        }
    }
}
