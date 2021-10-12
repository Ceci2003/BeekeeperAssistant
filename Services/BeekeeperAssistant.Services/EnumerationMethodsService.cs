namespace BeekeeperAssistant.Services
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    public class EnumerationMethodsService : IEnumerationMethodsService
    {
        public string GetDisplayName(Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .FirstOrDefault()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
    }
}
