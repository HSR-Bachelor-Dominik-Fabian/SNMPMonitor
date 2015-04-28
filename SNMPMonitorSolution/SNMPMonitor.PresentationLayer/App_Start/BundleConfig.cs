using System.Web;
using System.Web.Optimization;

namespace SNMPMonitor.PresentationLayer
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/Scripts/js").Include("~/Scripts/doT.min.js",
                "~/Scripts/jquery.signalR-2.2.0.min.js",
                "~/Scripts/highcharts.js",
                "~/Scripts/exporting.js",
                "~/Scripts/jasny-bootstrap.min.js",
                "~/Scripts/jquery.inputmask/jquery.inputmask.js",
                "~/Scripts/jquery.inputmask/jquery.inputmask.extensions.js",
                "~/Scripts/jquery.inputmask/jquery.inputmask.regex.extensions.js",
                "~/Scripts/jquery.inputmask/jquery.inputmask.numeric.extensions.js",
                "~/Scripts/sammy-latest.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap/theme/theme.less",
                      "~/Content/site.css",
                     "~/Content/mainLayout.less"));

            bundles.Add(new StyleBundle("~/Content/dasboardStyles").Include("~/Content/dashboardLayout.less"));

        }
    }
}
