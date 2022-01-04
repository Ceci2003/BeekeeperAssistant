using BeekeeperAssistant.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Web.Areas.App.Controllers
{
    [Area("App")]
    [Authorize]
    public class AppBaseController : BaseController
    {
    }
}
