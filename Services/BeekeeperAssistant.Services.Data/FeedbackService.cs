namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class FeedbackService : IFeedbackService
    {
        private readonly IDeletableEntityRepository<Feedback> feedbackRepository;

        public FeedbackService(IDeletableEntityRepository<Feedback> feedbackRepository)
        {
            this.feedbackRepository = feedbackRepository;
        }

        public async Task CreateAsync(string userId, string title, string body, FeedbackType feedbackType)
        {
            var feedback = new Feedback
            {
                UserId = userId,
                Title = title,
                Body = body,
                FeedbackType = feedbackType,
            };

            await this.feedbackRepository.AddAsync(feedback);
            await this.feedbackRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllFeedbackFeedbacks<T>(int? take = null, int skip = 0)
        {
            var feedbacks = this.feedbackRepository.All()
                .Where(f => f.FeedbackType == FeedbackType.Feedback)
                .Skip(skip);

            if (take.HasValue)
            {
                feedbacks = feedbacks.Take(take.Value);
            }

            return feedbacks.To<T>().ToList();
        }

        public int GetAllFeedbackFeedbacksCount()
            => this.feedbackRepository.All()
                .Where(f => f.FeedbackType == FeedbackType.Feedback)
                .Count();

        public IEnumerable<T> GetAllFeedbacks<T>(int? take = null, int skip = 0)
        {
            var feedbacks = this.feedbackRepository.All()
                .Skip(skip);

            if (take.HasValue)
            {
                feedbacks = feedbacks.Take(take.Value);
            }

            return feedbacks.To<T>().ToList();
        }

        public IEnumerable<T> GetAllHelpFeedbacks<T>(int? take = null, int skip = 0)
        {
            var feedbacks = this.feedbackRepository.All()
                .Where(f => f.FeedbackType == FeedbackType.Help)
                .Skip(skip);

            if (take.HasValue)
            {
                feedbacks = feedbacks.Take(take.Value);
            }

            return feedbacks.To<T>().ToList();
        }

        public int GetAllHelpFeedbacksCount()
            => this.feedbackRepository.All()
                .Where(f => f.FeedbackType == FeedbackType.Help)
                .Count();

        public T GetFeedbackById<T>(int id)
            => this.feedbackRepository
                .All()
                .Where(f => f.Id == id)
                .To<T>()
                .FirstOrDefault();
    }
}
