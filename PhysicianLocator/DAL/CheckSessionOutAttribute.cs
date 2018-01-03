using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PhysicianLocator.DAL
{
   
        [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
        public class CheckSessionOutAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var context = filterContext.HttpContext;
                if (context.Session != null)
                {
                    if (context.Session.IsNewSession)
                    {
                        string sessionCookie = context.Request.Headers["Cookie"];

                        if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                        {
                            FormsAuthentication.SignOut();
                            string redirectTo = "~/AccountManagment/Login";
                            if (!string.IsNullOrEmpty(context.Request.RawUrl))
                            {
                                redirectTo = string.Format("~/AccountManagment/Login",
                                    HttpUtility.UrlEncode(context.Request.RawUrl));

                            }
                        filterContext.HttpContext.Response.Redirect(redirectTo, true);

                    }
                    }
                }

                base.OnActionExecuting(filterContext);
            }
        }
    }
