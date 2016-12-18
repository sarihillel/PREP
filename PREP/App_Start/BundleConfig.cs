using System.Web;
using System.Web.Optimization;

namespace PREP
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundle
            bundles.UseCdn = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                       "~/Scripts/jquery-migrate-1.2.1.js",
                       "~/Scripts/jquery.validate*",
                       "~/Scripts/jquery.unobtrusive*",
                       "~/Scripts/jquery-ui-1.10.4.js"));

            bundles.Add(new ScriptBundle("~/bundles/EmpSearch").Include(
                    "~/Scripts/CommonEmployeeSearch.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"
                     ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapdatetimepicker").Include(
                      "~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrapdatetimepicker").Include(
                      "~/Content/bootstrap-datetimepicker.css"));

            bundles.Add(new StyleBundle("~/Releases/css").Include(
                      "~/Content/Releases/Header.css",
                      "~/Content/Releases/Footer.css",
                      "~/Content/Releases/ReleaseAreaOwner.css",
                      "~/Content/Releases/ReleaseStakeholder.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/Release").Include(
                     "~/Scripts/CheckPointDetails.js",
                     "~/Scripts/Releases/Details.js",
                     "~/Scripts/Footer.js"
                     ));

            bundles.Add(new StyleBundle("~/Shared/css").Include(
                       "~/Content/Releases/Navigation.css",                    
                     "~/Content/CustomDialog.css",
                     "~/Content/Shared/Layout.css",
                     "~/Content/Shared/site.css",
                     "~/Content/Sprite.css",
                       "~/Content/Shared/Responsive.css",
                        "~/Content/EmployeeSearch.css"
                     ));


            bundles.Add(new ScriptBundle("~/bundles/Shared").Include(
                    "~/Scripts/JavaScript.js",
                     "~/Scripts/CustomDialog.js",
                     "~/Scripts/CommonEmployeeSearch.js"
                ));


            bundles.Add(new ScriptBundle("~/bundles/Tables").Include(
                "~/Scripts/jquery.dataTables.js"
           //  , "~/Scripts/dataTables.bootstrap.js"
           ));

            bundles.Add(new StyleBundle("~/Content/Tables").Include(
                     "~/Content/DataTable.css"));
            //bundles.Add(new StyleBundle("~/Content/Status").Include(
            //        "~/Content/DataTable.css"));
            //bundles.Add(new ScriptBundle("~/bundles/Status").Include(
            //          "~/Scripts/Status/Status.js"
            //        ));
            bundles.Add(new ScriptBundle("~/bundles/EmployeeSearch").Include(
                     
                     ));
        }
    }
}

