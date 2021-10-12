namespace BeekeeperAssistant.Web.Components.Pages
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using BeekeeperAssistant.Services;
    using BeekeeperAssistant.Web.ViewModels.Apiaries;
    using Microsoft.AspNetCore.Components;

    public partial class AllApiariesComponent
    {
        [Parameter]
        public IEnumerable<ApiaryViewModel> AllUserApiaries { get; set; }

        [Parameter]
        public string AntiforgeryToken { get; set; }

        [Inject]
        public IEnumerationMethodsService EnumerationMethodsService { get; set; }

        public static string GetDisplayName(Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
    }
}
