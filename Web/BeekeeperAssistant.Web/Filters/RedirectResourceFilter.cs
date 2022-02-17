namespace BeekeeperAssistant.Web.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Filters;

    public class RedirectResourceFilter : IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            if (context.HttpContext.Request.Query.ContainsKey("returnUrl"))
            {
                if (context.HttpContext.Request.Method == "POST")
                {
                    var returnUrl = context.HttpContext.Request.Query["returnUrl"].ToString();

                    context.HttpContext.Response.Redirect(returnUrl);
                }
            }
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
        }
    }
}
