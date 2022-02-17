namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Web.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
