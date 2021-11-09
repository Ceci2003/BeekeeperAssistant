namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserController : AdministrationController
    {

        public IActionResult All()
        {
            return this.View();
        }
    }
}
