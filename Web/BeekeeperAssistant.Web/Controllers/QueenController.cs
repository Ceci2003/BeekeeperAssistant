namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Queens;
    using Microsoft.AspNetCore.Mvc;

    public class QueenController : Controller
    {
        private readonly IQueenService queenService;

        public QueenController(IQueenService queenService)
        {
            this.queenService = queenService;
        }

        public IActionResult All()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            return this.View();
        }

        public IActionResult ById(int id)
        {
            var queen = this.queenService.GetQueenById<QueenDataViewModel>(id);
            return this.View(queen);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateQueenInputModel inputModel)
        {
            var queenId = await this.queenService.CreateQueen(inputModel, 1);
            return this.Redirect($"/Queen/{queenId}");
        }
    }
}
