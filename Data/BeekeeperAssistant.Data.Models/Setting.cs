namespace BeekeeperAssistant.Data.Models
{
    using BeekeeperAssistant.Data.Common.Models;

    public class Setting : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
