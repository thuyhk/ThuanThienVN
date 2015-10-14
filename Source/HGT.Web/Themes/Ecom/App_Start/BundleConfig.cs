using System.Web;
using System.Web.Optimization;

namespace HGT.Web
{
    public partial class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void EComRegisterBundles(BundleCollection bundles)
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

            bundles.Add(new StyleBundle("~/Content/cssEcom").Include(
                     "~/Content/bootstrap.min.css",
                     "~/Themes/Ecom/Content/font-awesome.min.css",
                     "~/Themes/Ecom/Content/prettyPhoto.css",
                     "~/Themes/Ecom/Content/animate.css",
                     "~/Themes/Ecom/Content/style.css",
                     "~/Themes/Ecom/Content/menu_list.css",
                     "~/Themes/Ecom/Content/rwd.css",
                     "~/Themes/Ecom/Content/style.custom.css"));

            bundles.Add(new ScriptBundle("~/bundles/jsEcom").Include(
                      "~/scripts/jquery-2.1.1.js",
                      "~/scripts/bootstrap.min.js",
                      "~/Themes/Ecom/Content/scripts/jquery.scrollUp.min.js",
                      "~/Themes/Ecom/Content/scripts/jquery.prettyPhoto.js",
                      "~/Themes/Ecom/Content/scripts/script.js",
                      "~/scripts/grids.min.js"      
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
