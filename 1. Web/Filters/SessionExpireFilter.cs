using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MIB_FILM_CLD_MM_MVC.Filters
{
    public class SessionExpireFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var userJson = session.GetString("AclUser");

            if (string.IsNullOrEmpty(userJson))
            {
                var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                var redirectUrl = $"~/Home/Index?ReturnUrl={Uri.EscapeDataString(returnUrl)}";
                context.Result = new RedirectResult(redirectUrl);
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
