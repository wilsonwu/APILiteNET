using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APILiteNET.API
{
    public class RequiredAuthorzeAttribute : ActionFilterAttribute
    {
        public static readonly string ParemterSample = "platformcode";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Validate(filterContext.HttpContext.Request);
            base.OnActionExecuting(filterContext);
        }

        private static void Validate(HttpRequestBase request)
        {
            //Validate logic
        }
    }
}