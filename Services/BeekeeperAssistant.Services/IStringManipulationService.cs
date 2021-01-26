namespace BeekeeperAssistant.Services
{
    using System.Collections.Generic;
    using System.Text;

    public interface IStringManipulationService
    {
        string JoinStringWithSymbol(string first, string second, string symbol);
    }
}
