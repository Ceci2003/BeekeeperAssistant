namespace BeekeeperAssistant.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISystemNotificationService
    {
        Task<int> CreateAsync(string title, string content, string version, string imageUrl, string authorId);

        Task<int> EditAsync(int notificationId, string title, string content, string version, string imageUrl, string authorId);

        Task<int> DeleteAsync(int notificationId);

        IEnumerable<T> GetAllNotifications<T>();

        T GetNotificationById<T>(int notificationId);
    }
}
