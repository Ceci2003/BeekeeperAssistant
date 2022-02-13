namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;
    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Services.Messaging;
    using BeekeeperAssistant.Web.ViewModels.Administration.Feedbacks;
    using BeekeeperAssistant.Web.ViewModels.Feedbacks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class FeedbackController : AdministrationController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFeedbackService feedbackService;
        private readonly IEmailSender emailSender;

        public FeedbackController(
            UserManager<ApplicationUser> userManager,
            IFeedbackService feedbackService,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.feedbackService = feedbackService;
            this.emailSender = emailSender;
        }

        public IActionResult All(int pageFeedbacks = 1, int pageHelps = 1, int pageReports = 1)
        {
            if (pageFeedbacks <= 0)
            {
                pageFeedbacks = 1;
            }

            if (pageHelps <= 0)
            {
                pageHelps = 1;
            }

            var viewModel = new AdministrationAllFeedabackViewModel();

            viewModel.Feedbacks = new AllFeedbackFeedbacksViewModel();

            var allFeedbacksCount = this.feedbackService.GetAllFeedbackFeedbacksCount();
            var pagesFeedbacksCount = (int)Math.Ceiling((double)allFeedbacksCount / GlobalConstants.FeedbacksPerPage);

            if (pageFeedbacks <= 0)
            {
                pageFeedbacks = 1;
            }
            else if (pageFeedbacks > pagesFeedbacksCount)
            {
                pageFeedbacks = pagesFeedbacksCount == 0 ? 1 : pagesFeedbacksCount;
            }

            viewModel.Feedbacks.PagesCount = pagesFeedbacksCount == 0 ? 1 : pagesFeedbacksCount;

            viewModel.Feedbacks.CurrentPage = pageFeedbacks;

            var feedbacks = this.feedbackService.GetAllFeedbackFeedbacks<FeedbackDataAdministrationViewModel>(
                GlobalConstants.FeedbacksPerPage,
                (pageFeedbacks - 1) * GlobalConstants.FeedbacksPerPage);
            viewModel.Feedbacks.AllFeedbacks = feedbacks;

            // -------------------------------------------------
            viewModel.Helps = new AllFeedbackFeedbacksViewModel();

            var allHelpsCount = this.feedbackService.GetAllHelpFeedbacksCount();
            var pagesHelpsCount = (int)Math.Ceiling((double)allHelpsCount / GlobalConstants.FeedbacksPerPage);

            if (pageHelps <= 0)
            {
                pageHelps = 1;
            }
            else if (pageHelps > pagesHelpsCount)
            {
                pageHelps = pagesHelpsCount == 0 ? 1 : pagesHelpsCount;
            }

            viewModel.Helps.PagesCount = pagesHelpsCount == 0 ? 1 : pagesHelpsCount;

            viewModel.Helps.CurrentPage = pageHelps;
            var helps = this.feedbackService.GetAllHelpFeedbacks<FeedbackDataAdministrationViewModel>(
                GlobalConstants.FeedbacksPerPage,
                (pageHelps - 1) * GlobalConstants.FeedbacksPerPage);

            viewModel.Helps.AllFeedbacks = helps;

            // -------------------------------------------------
            viewModel.Reports = new AllFeedbackFeedbacksViewModel();

            var allReportsCount = this.feedbackService.GetAllReporsFeedbacksCount();
            var pagesReportsCount = (int)Math.Ceiling((double)allReportsCount / GlobalConstants.FeedbacksPerPage);

            if (pageReports <= 0)
            {
                pageReports = 1;
            }
            else if (pageReports > pagesReportsCount)
            {
                pageReports = pagesReportsCount == 0 ? 1 : pagesReportsCount;
            }

            viewModel.Reports.PagesCount = pagesReportsCount == 0 ? 1 : pagesReportsCount;

            viewModel.Reports.CurrentPage = pageReports;
            var reports = this.feedbackService.GetAllReportFeedbacks<FeedbackDataAdministrationViewModel>(
                GlobalConstants.FeedbacksPerPage,
                (pageHelps - 1) * GlobalConstants.FeedbacksPerPage);

            viewModel.Reports.AllFeedbacks = reports;

            return this.View(viewModel);
        }

        public async Task<IActionResult> ById(int id)
        {
            var viewModel = this.feedbackService.GetFeedbackById<AdministrationFeedbackAnswerInputViewModel>(id);

            var currentUser = await this.userManager.GetUserAsync(this.User);

            var removeIndex = viewModel.UserUserName.IndexOf("@");
            viewModel.UserUserName = viewModel.UserUserName.Remove(removeIndex);
            removeIndex = currentUser.UserName.IndexOf("@");
            viewModel.SenderName = currentUser.UserName.Remove(removeIndex);
            viewModel.SenderEmail = currentUser.Email;
            viewModel.Subject = $"Re: {viewModel.Title}";

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AnswerSendEmail(AdministrationFeedbackAnswerInputViewModel inputModel)
        {
            await this.emailSender.SendEmailAsync(
                  inputModel.SenderEmail,
                  inputModel.SenderName,
                  inputModel.UserEmail,
                  inputModel.Subject,
                  inputModel.AnswerContent);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
