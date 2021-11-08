namespace BeekeeperAssistant.Services.Data
{
    using System.Threading.Tasks;

    public interface IUserService
    {
        public int Count();

        Task<int> AdministratorsCount();
    }
}
