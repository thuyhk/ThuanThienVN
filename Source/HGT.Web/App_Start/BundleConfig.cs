using System.Web;
using System.Web.Optimization;

namespace HGT.Web
{
    public partial class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryVal").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/bootstrap.min.css",
                     "~/Content/bootstrap.custom.css",
                     "~/Content/font-awesome/css/font-awesome.min.css",
                     "~/Content/animate.css",
                     "~/Content/font-reg.css",
                     "~/Content/style.css",
                     "~/Content/rwd.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                      "~/scripts/jquery-1.10.2.js",
                      "~/scripts/bootstrap.min.js",
                      "~/scripts/respond.js",
                      "~/scripts/grids.min.js",                      
                      "~/scripts/jquery.common.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/kendo_css").Include(
                      "~/Content/kendo/2014.1.528/kendo.common.min.css",
                      "~/Content/kendo/2014.1.528/kendo.dataviz.min.css",
                      "~/Content/kendo/2014.1.528/kendo.silver.min.css",
                      "~/Content/kendo/2014.1.528/kendo.dataviz.default.min.css",
                      "~/Areas/Admin/Content/kendo.custom.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/kendo_script").Include(
                      "~/Scripts/kendo/2014.1.528/kendo.all.min.js",
                      "~/Scripts/kendo/2014.1.528/kendo.aspnetmvc.min.js",
                      "~/Areas/Admin/Scripts/kendo.custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/jsValidation").Include(
                      "~/Scripts/jquery.unobtrusive-ajax.js",
                      "~/Scripts/jquery.validate.js",
                      "~/Scripts/jquery.validate.unobtrusive.js",
                      "~/Scripts/jquery.validate.unobtrusive.bootstrap.tootip.js"));

            bundles.Add(new ScriptBundle("~/bundles/jsEditor").Include(
                     "~/Utility/ckeditor/ckeditor.js",
                     "~/Utility/ckfinder/ckfinder.js",
                     "~/Utility/hgt.editor.js",
                     "~/Utility/init.editor.js"));
        }
    }
}
