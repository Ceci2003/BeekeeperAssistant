using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeekeeperAssistant.Web.Filters
{
    public class RedirectActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
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

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
