namespace BeekeeperAssistant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IApiaryHelperService
    {
        Task Add(string userId, int apiaryId);
    }
}
