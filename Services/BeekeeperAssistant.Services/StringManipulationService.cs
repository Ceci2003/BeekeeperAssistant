using System;

namespace BeekeeperAssistant.Services
{
    public class StringManipulationService : IStringManipulationService
    {
        public string JoinStringWithSymbol(string first, string second, string symbol)
        {
            var joindeString = $"{first}{symbol}{second}";
            return joindeString;
        }
    }
}
