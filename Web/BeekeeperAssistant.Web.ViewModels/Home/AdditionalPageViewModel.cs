namespace BeekeeperAssistant.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using BeekeeperAssistant.Web.ViewModels.SystemNotification;

    public class AdditionalPageViewModel
    {
        public IEnumerable<SystemNotificationViewModel> SystemNotifications { get; set; }
    }
}
