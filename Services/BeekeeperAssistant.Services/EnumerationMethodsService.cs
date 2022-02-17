namespace BeekeeperAssistant.Services
{
    using System;
    using System.Collections.Generic;
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

        public List<KeyValuePair<string, int>> GetEnumList<T>()
        {
            var list = new List<KeyValuePair<string, int>>();
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                list.Add(new KeyValuePair<string, int>(e.ToString(), (int)e));
            }

            return list;
        }

        public bool IsEnumerationDefined(Enum @enum)
        {
            if (Enum.IsDefined(@enum.GetType(), @enum))
            {
                return true;
            }

            return false;
        }
    }
}
