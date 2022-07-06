namespace BeekeeperAssistant.Web.ViewModels.SystemNotification
{
    using System;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class SystemNotificationViewModel : IMapFrom<SystemNotification>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Version { get; set; }

        public string ImageUrl { get; set; }

        public string AuthorUserName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
