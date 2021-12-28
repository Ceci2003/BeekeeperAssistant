using System.Collections.Generic;
using System.Text;

namespace BeekeeperAssistant.Services
{
    public interface IStringManipulationService
    {
        string Slugify(string input);

        string Unslugify(string input);

    }
}
