namespace BeekeeperAssistant.Services
{
    using System.Collections.Generic;
    using System.Text;

    public interface IStringManipulationService
    {
        string Slugify(string input);

        string Unslugify(string input);
    }
}
