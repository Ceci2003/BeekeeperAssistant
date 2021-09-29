namespace BeekeeperAssistant.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class ApiaryApiController : ControllerBase
    {
        [HttpGet]
        public ActionResult All()
        {
            return this.Ok();
        }
    }
}
