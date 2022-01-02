namespace BeekeeperAssistant.Web.Controllers
{
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.BeehiveNotes;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class BeehiveNoteController : BaseController
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

        public async Task<IActionResult> AllByBeehiveId(int beehiveId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var viewModel = new AllByBeehiveIdBeehiveNoteViewModel();

            viewModel.AllNotes = this.beehiveNoteService.GetAllBeehiveNotes<ByBeehiveIdBeehiveNoteViewModel>(beehiveId);
            viewModel.BeehiveAccess = await this.beehiveHelperServic.GetUserBeehiveAccessAsync(currentUser.Id, beehiveId);

            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataModel>(beehiveId);
            viewModel.BeehiveId = beehiveId;
            viewModel.Number = beehive.Number;
            viewModel.ApiaryId = beehive.Apiary.Id;
            viewModel.ApiaryName = beehive.Apiary.Name;
            viewModel.ApiaryNumber = beehive.Apiary.Number;

            return this.View(viewModel);
        }

        public IActionResult Create(int beehiveId)
        {
            var viewModel = new CreateBeehiveNoteInputModel();

            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataModel>(beehiveId);
            viewModel.BeehiveId = beehive.Id;
            viewModel.Number = beehive.Number;
            viewModel.ApiaryId = beehive.Apiary.Id;
            viewModel.ApiaryNumber = beehive.Apiary.Number;
            viewModel.ApiaryName = beehive.Apiary.Name;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int beehiveId, CreateBeehiveNoteInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.beehiveNoteService.CreateAsync(
                    beehiveId,
                    inputModel.Title,
                    inputModel.Content,
                    inputModel.Color,
                    currentUser.Id);

            return this.RedirectToAction("AllByBeehiveId", "BeehiveNote", new { beehiveId = beehiveId });
        }

        public IActionResult Edit(int noteId)
        {
            var viewModel = this.beehiveNoteService.GetBeehiveNoteById<EditBeehiveNoteInputModel>(noteId);

            var beehive = this.beehiveService.GetBeehiveById<BeehiveDataModel>(viewModel.BeehiveId);
            viewModel.Number = beehive.Number;
            viewModel.ApiaryId = beehive.Apiary.Id;
            viewModel.ApiaryNumber = beehive.Apiary.Number;
            viewModel.ApiaryName = beehive.Apiary.Name;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int noteId, EditBeehiveNoteInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var beehiveId = await this.beehiveNoteService.EditAsync(
                noteId,
                inputModel.Title,
                inputModel.Content,
                inputModel.Color,
                currentUser.Id);

            return this.RedirectToAction("AllByBeehiveId", "BeehiveNote", new { beehiveId = beehiveId });
        }

        public async Task<IActionResult> Delete(int noteId)
        {
            var beehiveId = await this.beehiveNoteService.DeleteAsync(noteId);

            return this.RedirectToAction("AllByBeehiveId", "BeehiveNote", new { beehiveId = beehiveId });
        }
    }
}
