using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWorksHandler.WEB.Extensions
{
    public static class RazorExtensions
    {
        public static string AbsoluteAction(
            this IUrlHelper url,
            string actionName,
            string controllerName,
            object routeValues = null)
        {
            string scheme = url.ActionContext.HttpContext.Request.Scheme;
            return url.Action(actionName, controllerName, routeValues, scheme);
        }
    }
}
