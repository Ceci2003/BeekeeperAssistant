using System;
using System.Collections.Generic;
using System.Text;

namespace BeekeeperAssistant.Services
{
    public interface ITypeService
    {
        string[] GetAllTypePropertiesName(Type type);

        string[] GetAllTypePropertiesDisplayName(Type type);
    }
}
