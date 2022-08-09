namespace BeekeeperAssistant.Web.ViewModels.UserTasks
{
    using System;

    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class UserTaskViewModel : IMapFrom<UserTask>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Color { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
