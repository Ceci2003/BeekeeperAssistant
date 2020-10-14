using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Web.Controllers
{
    public class ApiaryController : Controller
    {

        public IActionResult GetByNumber(string apiNumber)
        {
            return this.View();
        }
    }
}
