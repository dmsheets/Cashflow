using System.Web;
using System.Web.Optimization;

namespace CashCard
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js")); 
            
            bundles.Add(new ScriptBundle("~/bundles/jquerynumber").Include(
              "~/Scripts/jquery.number.js"));

            bundles.Add(new ScriptBundle("~/bundles/numeral").Include(
              "~/Scripts/numeral/numeral.js"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-{version}.js"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-select.js",
                      "~/Scripts/bootstrap-datepicker.js"));

            bundles.Add(new StyleBundle("~/content/main").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/bootstrap-select.min.css",
                "~/Content/bootstrap-datepicker3.css",
                "~/Content/sb-admin.css",
                "~/Content/font-awesome.css"));

            //bundles.Add(new StyleBundle("~/content/css").
            //          Include("~/Content/font-awesome.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/content/picker").Include(
                                "~/Content/themes/base/accordion.css",
                                "~/Content/themes/base/all.css",
                                "~/Content/themes/base/autocomplete.css",
                                "~/Content/themes/base/base.css",
                                "~/Content/themes/base/button.css",
                                "~/Content/themes/base/core.css",
                                "~/Content/themes/base/datepicker.css",
                                "~/Content/themes/base/dialog.css",
                                "~/Content/themes/base/draggable.css",
                                "~/Content/themes/base/menu.css",
                                "~/Content/themes/base/progressbar.css",
                                "~/Content/themes/base/resizable.css",
                                "~/Content/themes/base/selectable.css",
                                "~/Content/themes/base/selectmenu.css",
                                "~/Content/themes/base/slider.css",
                                "~/Content/themes/base/sortable.css",
                                "~/Content/themes/base/spinner.css",
                                "~/Content/themes/base/tabs.css",
                                "~/Content/themes/base/theme.css",
                                "~/Content/themes/base/tooltip.css"
                                ));
            
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                "~/Scripts/DataTables/jquery.dataTables.js",
                "~/Scripts/DataTables/dataTables.bootstrap.js",
                 "~/Scripts/DataTables/dataTables.tableTools.js"));

            bundles.Add(new StyleBundle("~/content/datatable").Include(
                "~/Content/DataTables/css/dataTables.bootstrap.css",
                "~/Content/DataTables/css/dataTables.tableTools.css"));

        
        }
    }
}
