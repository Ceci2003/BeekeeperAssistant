namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Web.ViewModels.Queen;
    using Microsoft.AspNetCore.Mvc;

    public class QueenController : Controller
    {
        public IActionResult All()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateQueenInputModel inputModel)
        {
            return this.Redirect("/");
        }
    }
}
