namespace BeekeeperAssistant.Services
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    public class TypeService : ITypeService
    {
        public string[] GetAllTypePropertiesDisplayName(Type type)
        {
            var allPropertiesDisplayNames = type.GetProperties()
                .Select(x => x.GetCustomAttribute<DisplayAttribute>().GetName())
                .ToArray();

            return allPropertiesDisplayNames;
        }

        public string[] GetAllTypePropertiesName(Type type)
        {
            var allProperties = type.GetProperties().Select(t => t.Name).ToArray();

            return allProperties;
        }
    }
}
