namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Beehives;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [Authorize]
    public class BeehiveController : BaseController
    {
        private readonly IDeletableEntityRepository<Beehive> beehiveRepository;
        private readonly IBeehiveService beehiveService;

        public BeehiveController(
            IDeletableEntityRepository<Beehive> beehiveRepository,
            IBeehiveService beehiveService)
        {
            this.beehiveRepository = beehiveRepository;
            this.beehiveService = beehiveService;
        }

        // Does not work!
        public IActionResult GetByNumber()
        {
            return this.View();
        }

        // Add Action Create
        public IActionResult Create(int id)
        {
            this.ViewData["ApiaryId"] = id;
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBeehiveInputModel inputModel)
        {
            var beehive = new Beehive()
            {
                ApiaryId = inputModel.ApiaryId,
                BeehivePower = inputModel.BeehivePower,
                BeehiveSystem = inputModel.BeehiveSystem,
                BeehiveType = inputModel.BeehiveType,
                Date = inputModel.Date,
                Number = inputModel.Number,
                HasDevice = inputModel.HasDevice,
                HasPolenCatcher = inputModel.HasPolenCatcher,
                HasPropolisCatcher = inputModel.HasPropolisCatcher,
            };

            await this.beehiveRepository.AddAsync(beehive);
            await this.beehiveRepository.SaveChangesAsync();

            return this.Redirect("/");
        }

        // Add Action All
    }
}
