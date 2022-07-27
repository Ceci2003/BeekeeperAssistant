namespace BeekeeperAssistant.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ITypeService
    {
        string[] GetAllTypePropertiesName(Type type);

        string[] GetAllTypePropertiesDisplayName(Type type);
    }
}
