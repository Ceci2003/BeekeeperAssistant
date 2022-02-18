namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.BeehiveMarkFlags;
    using Microsoft.AspNetCore.Mvc;

    public class BeehiveMarkFlagController : AppBaseController
    {
        private readonly IBeehiveMarkFlagService beehiveMarkFlagService;
        private readonly IBeehiveService beehiveService;
        private readonly IApiaryService apiaryService;

        public BeehiveMarkFlagController(
            IBeehiveMarkFlagService beehiveMarkFlagService,
            IBeehiveService beehiveService,
            IApiaryService apiaryService)
        {
            this.beehiveMarkFlagService = beehiveMarkFlagService;
            this.beehiveService = beehiveService;
            this.apiaryService = apiaryService;
        }

        public IActionResult Create(int id, MarkFlagType markFlagType)
        {
            var inputModel = new CreateBeehiveMarkFlagInputModel()
            {
                FlagType = markFlagType,
                BeehiveId = id,
                BeehiveNumber = this.beehiveService.GetBeehiveNumberById(id),
                BeehiveApiaryId = this.apiaryService.GetApiaryIdByBeehiveId(id),
                BeehiveApiaryName = this.apiaryService.GetApiaryNameByBeehiveId(id),
                BeehiveApiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(id),
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id, CreateBeehiveMarkFlagInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.beehiveMarkFlagService.CreateBeehiveFlag(id, inputModel.Content, inputModel.FlagType);

            return this.RedirectToAction("ById", "Beehive", new { id = id });
        }

        public IActionResult Edit(int id, int beehiveId)
        {
            var inputModel = this.beehiveMarkFlagService.GetBeehiveFlagByFlagId<EditBeehiveMarkFlagInputModel>(id);

            inputModel.BeehiveId = beehiveId;
            inputModel.BeehiveNumber = this.beehiveService.GetBeehiveNumberById(beehiveId);
            inputModel.BeehiveApiaryId = this.apiaryService.GetApiaryIdByBeehiveId(beehiveId);
            inputModel.BeehiveApiaryName = this.apiaryService.GetApiaryNameByBeehiveId(beehiveId);
            inputModel.BeehiveApiaryNumber = this.apiaryService.GetApiaryNumberByBeehiveId(beehiveId);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditBeehiveMarkFlagInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.beehiveMarkFlagService.EditBeehiveFlag(id, inputModel.Content, inputModel.FlagType);

            return this.RedirectToAction("ById", "Beehive", new { id = inputModel.BeehiveId });
        }

        public async Task<IActionResult> Delete(int id, int beehiveId)
        {
            await this.beehiveMarkFlagService.DeleteBeehiveFlag(id);

            return this.RedirectToAction("ById", "Beehive", new { id = beehiveId });
        }
    }
}
