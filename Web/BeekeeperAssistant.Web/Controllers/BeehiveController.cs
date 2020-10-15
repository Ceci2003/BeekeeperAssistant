namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class BeehiveController : BaseController
    {
        // Does not work!
        public IActionResult GetByNumber()
        {
            return this.View();
        }
    }
}
