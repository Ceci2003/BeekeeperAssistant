namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class QueenController : Controller
    {
        public IActionResult All(int page = 1)
        {
            return this.View();
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(string hereGoesInutModel)
        {
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
