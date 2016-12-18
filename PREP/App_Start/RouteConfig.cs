using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PREP
{

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: null,
              url: "Status/ViewStatus/{ReleaseId}",
              defaults: new { controller = "Status", action = "ViewStatus" }
            );
            routes.MapRoute(
                name: null,
                url: "CheckList/ViewCheckList/{ReleaseID}",
                defaults: new { controller = "CheckList", action = "ViewCheckList" }
            );
            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
                name: "ReleaseDetails",
                url: "{controller}/{action}/{ModeName}/{ReleaseId}",
                defaults: new { controller = "Releases", action = "Details", ModeName = "ADD", ReleaseId = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "ReleaseDetailsTabIndex",
              url: "{controller}/{action}/{ModeName}/{ReleaseId}/{TabIndex}",
              defaults: new { controller = "Releases", action = "Details", ModeName = "ADD", id = UrlParameter.Optional }
          );


        }
    }
}
