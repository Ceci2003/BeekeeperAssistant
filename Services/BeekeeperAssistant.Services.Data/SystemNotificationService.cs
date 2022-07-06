namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BeekeeperAssistant.Data.Common.Repositories;
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class SystemNotificationService : ISystemNotificationService
    {
        private readonly IDeletableEntityRepository<SystemNotification> notificationRepository;

        public SystemNotificationService(
            IDeletableEntityRepository<SystemNotification> notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        public async Task<int> CreateAsync(string title, string content, string version, string imageUrl, string authorId)
        {
            var notification = new SystemNotification
            {
                Title = title,
                Content = content,
                Version = version,
                ImageUrl = imageUrl,
                AuthorId = authorId,
            };

            await this.notificationRepository.AddAsync(notification);
            await this.notificationRepository.SaveChangesAsync();

            return notification.Id;
        }

        public async Task<int> EditAsync(int notificationId, string title, string content, string version, string imageUrl, string authorId)
        {
            var notification = this.notificationRepository
                .All()
                .FirstOrDefault(n => n.Id == notificationId);

            notification.Title = title;
            notification.Content = content;
            notification.Version = version;
            notification.ImageUrl = imageUrl;

            await this.notificationRepository.SaveChangesAsync();
            return notification.Id;
        }

        public async Task<int> DeleteAsync(int notificationId)
        {
            var notification = this.notificationRepository
                .All()
                .FirstOrDefault(n => n.Id == notificationId);

            this.notificationRepository.Delete(notification);
            await this.notificationRepository.SaveChangesAsync();

            return notification.Id;
        }

        public IEnumerable<T> GetAllNotifications<T>()
        {
            var notifications = this.notificationRepository
                .All()
                .OrderByDescending(n => n.CreatedOn)
                .Take(10)
                .To<T>()
                .ToList();

            return notifications;
        }

        public T GetNotificationById<T>(int notificationId)
        {
            var notification = this.notificationRepository
                .All()
                .Where(n => n.Id == notificationId)
                .To<T>()
                .FirstOrDefault();

            return notification;
        }
    }
}
