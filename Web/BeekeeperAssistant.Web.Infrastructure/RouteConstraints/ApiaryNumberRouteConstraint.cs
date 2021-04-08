namespace BeekeeperAssistant.Web.Infrastructure.RouteConstraints
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class ApiaryNumberRouteConstraint : IRouteConstraint
    {
        public bool Match(
            HttpContext httpContext,
            IRouter route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            var apiaryNumber = values.FirstOrDefault(x => x.Key == routeKey).Value?.ToString();

            if (Regex.IsMatch(apiaryNumber, @"\b([\d]{4}\b)-(\b\d{4})\b"))
            {
                return true;
            }

            return false;
        }
    }
}
