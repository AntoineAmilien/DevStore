using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace DevStore.ActionFilters
{
    public class SessionAdminOut : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext httpContext = filterContext.HttpContext;
            if (!httpContext.Session.Keys.Contains("SessionUser"))
            {
                filterContext.Result = new RedirectResult("~/Home/TimeOut");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
