namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Administration.Communication;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    public class CommunicationController : AdministrationController
    {
        private readonly IConfiguration configuration;
        private readonly ICommunicationService communicationService;

        public CommunicationController(
            IConfiguration configuration,
            ICommunicationService communicationService)
        {
            this.configuration = configuration;
            this.communicationService = communicationService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult SendEmail(string toOne = null)
        {
            var inputModel = new SendEmailInputModel()
            {
                FromEmail = this.configuration["SendGrid:SenderEmail"],
                FromName = "BeekeeperAssistant - Администратор",
            };

            if (!string.IsNullOrWhiteSpace(toOne))
            {
                inputModel.To = toOne;
                inputModel.SendOptions = SendEmailOptions.ToOne;
            }

            return this.View(inputModel);
        }

        [HttpPost]
        public IActionResult SendEmail(SendEmailInputModel inputModel)
        {
            this.communicationService.SendEmail(
                inputModel.FromEmail,
                inputModel.FromName,
                inputModel.SendOptions,
                inputModel.To,
                inputModel.ToMultiple,
                inputModel.Subject,
                inputModel.HtmlContent);

            this.TempData[GlobalConstants.SuccessMessage] = $"Имейлът беше успешно изпратен!";

            return this.View(nameof(this.Index));
        }
    }
}
