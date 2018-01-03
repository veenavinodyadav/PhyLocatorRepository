using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhysicianLocator.DAL
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            // check  sessions here
            if (HttpContext.Current.Session["username"] == null)
            {
                filterContext.Result = new RedirectResult("~/AccountManagment/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}