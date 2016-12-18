using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PREP.Functions
{

    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // The user is not authenticated
                base.HandleUnauthorizedRequest(filterContext);
            }
            else if (!this.Roles.Split(',').Any(filterContext.HttpContext.User.IsInRole))
            {
                // The user is not in any of the listed roles => 
                // show the unauthorized view
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Unauthorized.cshtml"
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    //public class AuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    //{
    //    private MyCustomMode _Mode;
    //    public AuthorizationAttribute(MyCustomMode mode)
    //    {
    //        _Mode = mode;
    //    }
    //    //public virtual void OnAuthorization(AuthorizationContext filterContext)
    //    //{
    //    //    var Groups = AuthLDAP.GetPrrGroups();
    //    //    if (filterContext == null)
    //    //    {
    //    //        throw new ArgumentNullException("filterContext");
    //    //    }
    //    //    // run my own logic here.
    //    //    // set the filterContext.Result to anything non-null (such as
    //    //    // a RedirectResult?) to skip the action method's execution.
    //    //    //
    //    //  }  //

    //    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    //    {
    //        if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
    //        {
    //            // The user is not authenticated
    //            base.HandleUnauthorizedRequest(filterContext);
    //        }
    //        else if (!this.Roles.Split(',').Any(filterContext.HttpContext.User.IsInRole))
    //        {
    //            // The user is not in any of the listed roles => 
    //            // show the unauthorized view
    //            filterContext.Result = new ViewResult
    //            {
    //                ViewName = "~/Views/Shared/Unauthorized.cshtml"
    //            };
    //        }
    //        else
    //        {
    //            base.HandleUnauthorizedRequest(filterContext);
    //        }
        
    //}

    public enum MyCustomMode
    {
        Enforce,
        Ignore
    }
}