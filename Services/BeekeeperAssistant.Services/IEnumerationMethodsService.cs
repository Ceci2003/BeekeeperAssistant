namespace BeekeeperAssistant.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IEnumerationMethodsService
    {
        string GetDisplayName(Enum enumValue);

        bool IsEnumerationDefined(Enum @enum);

        public List<KeyValuePair<string, int>> GetEnumList<T>();
    }
}
