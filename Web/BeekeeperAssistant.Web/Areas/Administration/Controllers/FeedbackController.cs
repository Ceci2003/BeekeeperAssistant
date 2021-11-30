namespace BeekeeperAssistant.Web.Areas.Administration.Controllers
{
    using BeekeeperAssistant.Common;
    using BeekeeperAssistant.Services.Data;
    using BeekeeperAssistant.Web.ViewModels.Feedbacks;
    using Microsoft.AspNetCore.Mvc;
    using System;

    public class FeedbackController : AdministrationController
    {
        private readonly IFeedbackService feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            this.feedbackService = feedbackService;
        }

        public IActionResult All(int pageFeedbacks = 1, int pageHelps = 1)
        {
            var viewModel = new AllFeedabacksAdministrationViewModel();

            viewModel.Feedbacks = new AllFeedbackFeedbacksViewModel();

            var allFeedbacksCount = this.feedbackService.GetAllFeedbackFeedbacksCount();
            var pagesFeedbacksCount = (int)Math.Ceiling((double)allFeedbacksCount / GlobalConstants.ApiariesPerPage);

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
                GlobalConstants.ApiariesPerPage,
                (pageFeedbacks - 1) * GlobalConstants.ApiariesPerPage);
            viewModel.Feedbacks.AllFeedbacks = feedbacks;

            // -------------------------------------------------
            viewModel.Helps = new AllFeedbackFeedbacksViewModel();

            var allHelpsCount = this.feedbackService.GetAllHelpFeedbacksCount();
            var pagesHelpsCount = (int)Math.Ceiling((double)allHelpsCount / GlobalConstants.ApiariesPerPage);

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
                GlobalConstants.ApiariesPerPage,
                (pageHelps - 1) * GlobalConstants.ApiariesPerPage);

            viewModel.Helps.AllFeedbacks = helps;

            return this.View(viewModel);
        }

        public IActionResult ById(int id)
        {
            var viewModel = this.feedbackService.GetFeedbackById<FeedbackViewModel>(id);

            return this.View(viewModel);
        }
    }
}
