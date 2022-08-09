namespace BeekeeperAssistant.Web.ViewModels.UserTasks
{
    using System.Collections.Generic;

    public class AllByUserIdUserTaskViewModel
    {
        public IEnumerable<UserTaskViewModel> UserTasks { get; set; }

        public CreateUserTaskInputModel CreateModel { get; set; }

        public EditUserTaskInputModel EditModel { get; set; }
    }
}
