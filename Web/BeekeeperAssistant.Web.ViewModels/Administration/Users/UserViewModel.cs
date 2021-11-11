namespace BeekeeperAssistant.Web.ViewModels.Administration.Users
{
    using BeekeeperAssistant.Data.Models;
    using BeekeeperAssistant.Services.Mapping;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsDeleted { get; set; }

    }
}
