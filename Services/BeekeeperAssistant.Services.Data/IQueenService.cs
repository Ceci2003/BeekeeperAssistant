using BeekeeperAssistant.Web.ViewModels.Queens;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Services.Data
{
    public interface IQueenService
    {

        Task<int> CreateQueen(CreateQueenInputModel inputModel,int beehiveId);

        T GetQueenById<T>(int id);

    }
}
