namespace BeekeeperAssistant.Services
{
    using System;

    public class StringManipulationService : IStringManipulationService
    {
        public string Slugify(string input)
        {
            var output = input.ToLower();
            var charsToReplace = ".";

            foreach (var @char in charsToReplace)
            {
                output = output.Replace(@char, '-');
            }

            return output;
        }

        public string Unslugify(string input)
        {
            var output = input.Split("-");

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = output[i][0] + output[i].Substring(1);
            }

            return string.Join(".", output);
        }
    }
}
