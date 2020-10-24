using System.Collections.Generic;
using System.Text;

namespace BeekeeperAssistant.Services
{
    public interface IStringManipulationService
    {
        string JoinStringWithSymbol(string first, string second, string symbol);
    }
}
