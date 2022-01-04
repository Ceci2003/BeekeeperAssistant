namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.BeehiveNotes;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class BeehiveNoteController : AppBaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBeehiveNoteService beehiveNoteService;
        private readonly IBeehiveHelperService beehiveHelperServic;
        private readonly IBeehiveService beehiveService;

        public BeehiveNoteController(
            UserManager<ApplicationUser> userManager,
            IBeehiveNoteService beehiveNoteService,
            IBeehiveHelperService beehiveHelperServic,
            IBeehiveService beehiveService)
        {
            this.userManager = userManager;
            this.beehiveNoteService = beehiveNoteService;
            this.beehiveHelperServic = beehiveHelperServic;
            this.beehiveService = beehiveService;
        }

        public async Task<IActionResult> AllByBeehiveId(int id)
        {
            var currentUser = await userManager.GetUserAsync(User);
            var viewModel = new AllByBeehiveIdBeehiveNoteViewModel();

            viewModel.AllNotes = beehiveNoteService.GetAllBeehiveNotes<ByBeehiveIdBeehiveNoteViewModel>(id);
            viewModel.BeehiveAccess = await beehiveHelperServic.GetUserBeehiveAccessAsync(currentUser.Id, id);

            var beehive = beehiveService.GetBeehiveById<BeehiveDataModel>(id);
            viewModel.BeehiveId = id;
            viewModel.Number = beehive.Number;
            viewModel.ApiaryId = beehive.Apiary.Id;
            viewModel.ApiaryName = beehive.Apiary.Name;
            viewModel.ApiaryNumber = beehive.Apiary.Number;

            return View(viewModel);
        }

        public IActionResult Create(int id)
        {
            var viewModel = new CreateBeehiveNoteInputModel();

            var beehive = beehiveService.GetBeehiveById<BeehiveDataModel>(id);
            viewModel.BeehiveId = beehive.Id;
            viewModel.Number = beehive.Number;
            viewModel.ApiaryId = beehive.Apiary.Id;
            viewModel.ApiaryNumber = beehive.Apiary.Number;
            viewModel.ApiaryName = beehive.Apiary.Name;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id, CreateBeehiveNoteInputModel inputModel)
        {
            var currentUser = await userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            await beehiveNoteService.CreateAsync(
                    id,
                    inputModel.Title,
                    inputModel.Content,
                    inputModel.Color,
                    currentUser.Id);

            return RedirectToAction("AllByBeehiveId", "BeehiveNote", new { beehiveId = id });
        }

        public IActionResult Edit(int id)
        {
            var viewModel = beehiveNoteService.GetBeehiveNoteById<EditBeehiveNoteInputModel>(id);

            var beehive = beehiveService.GetBeehiveById<BeehiveDataModel>(viewModel.BeehiveId);
            viewModel.Number = beehive.Number;
            viewModel.ApiaryId = beehive.Apiary.Id;
            viewModel.ApiaryNumber = beehive.Apiary.Number;
            viewModel.ApiaryName = beehive.Apiary.Name;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditBeehiveNoteInputModel inputModel)
        {
            var currentUser = await userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            var beehiveId = await beehiveNoteService.EditAsync(
                id,
                inputModel.Title,
                inputModel.Content,
                inputModel.Color,
                currentUser.Id);

            return RedirectToAction("AllByBeehiveId", "BeehiveNote", new { beehiveId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var beehiveId = await beehiveNoteService.DeleteAsync(id);

            return RedirectToAction("AllByBeehiveId", "BeehiveNote", new { beehiveId });
        }
    }
}
