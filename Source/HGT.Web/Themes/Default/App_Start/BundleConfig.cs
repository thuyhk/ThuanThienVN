using System.Web;
using System.Web.Optimization;

namespace HGT.Web
{
    public partial class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void SharpRegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jsBootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/cssDefault").Include(
                     "~/Content/bootstrap.css",
                     "~/Themes/Default/Content/style.css",
                     "~/Themes/Default/Content/superfish.css",
                     "~/Themes/Default/Content/resize.css",
                     "~/Content/style.edit.css",
                     "~/Content/fontreg.css"));

            bundles.Add(new ScriptBundle("~/bundles/jsDefault").Include(
                      "~/Scripts/jquery-1.8.0.min.js",
                      "~/Scripts/nicescroll.min.js",
                      "~/Scripts/grids.min.js",
                      "~/Scripts/script.js",
                      "~/Scripts/flaunt.js",
                      "~/Scripts/easyResponsiveTabs.js"                      
                      ));

            bundles.Add(new StyleBundle("~/Content/kendo_css").Include(
                      "~/Content/kendo/2014.1.528/kendo.common.min.css",
                      "~/Content/kendo/2014.1.528/kendo.dataviz.min.css",
                      "~/Content/kendo/2014.1.528/kendo.silver.min.css",
                      "~/Content/kendo/2014.1.528/kendo.dataviz.default.min.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/kendo_script").Include(
                      "~/Scripts/kendo/2014.1.528/kendo.all.min.js",
                      "~/Scripts/kendo/2014.1.528/kendo.aspnetmvc.min.js",
                      "~/Areas/Admin/Scripts/kendo.common.js"));

            bundles.Add(new ScriptBundle("~/bundles/jsValidation").Include(
                      "~/Scripts/jquery.unobtrusive-ajax.js",
                      "~/Scripts/jquery.validate.js",
                      "~/Scripts/jquery.validate.unobtrusive.js"));
        }
    }
}
