using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Web.Mvc;

namespace PREP.Models
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
#if !DEBUG
        const string sourceName = "PREP";
        public override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");
            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };
            if (!EventLog.SourceExists(sourceName))
                EventLog.CreateEventSource(sourceName, "Application");
            EventLog.WriteEntry(sourceName, string.Format("{0} {1}User: {2}", filterContext.Exception.ToString(), Environment.NewLine, filterContext.HttpContext.User.Identity.Name), EventLogEntryType.Error);
            base.OnException(filterContext);
        }
#endif 
    }
}