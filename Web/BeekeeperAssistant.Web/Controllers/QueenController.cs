namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Web.ViewModels.Queens;
    using Microsoft.AspNetCore.Mvc;

    public class QueenController : Controller
    {
        public IActionResult All(int page = 1)
        {
            return this.View();
        }

        public IActionResult ById(int id)
        {
            return this.View();
        }

        public IActionResult Create(int id)
        {
            var inputModel = new CreateQueenInputModel
            {
                BeehiveId = id,
            };

            return this.View(inputModel);
        }

        [HttpPost]
        public IActionResult Create(CreateQueenInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            return this.Redirect("/");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            return this.Redirect("/");
        }

        public IActionResult Edit(int id)
        {
            return this.View();
        }

        public IActionResult Edit(string hereGoesInputModel)
        {
            return this.Redirect("/");
        }
    }
}
