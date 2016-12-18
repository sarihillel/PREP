using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PREP 
{
    public class MvcApplication : System.Web.HttpApplication
    {
        const string sourceName = "PREP";
#if !DEBUG
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (!EventLog.SourceExists(sourceName))
                EventLog.CreateEventSource(sourceName, "Application");
            EventLog.WriteEntry(sourceName, string.Format("{0} {1}User: {2}", exception, Environment.NewLine, Context.User.Identity.Name), EventLogEntryType.Error);
            Server.ClearError();
           // Response.Redirect("/Shared/Error");
        }
#endif
        protected void Application_Start()
        {
           // GlobalFilters.Filters.Add(new RequireHttpsAttribute());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
