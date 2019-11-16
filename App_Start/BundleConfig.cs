using System.Web;
using System.Web.Optimization;

namespace NAPASTUDENT
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/SiteScript/Global.js"
                ));
            
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-validate/jquery.validate.min.js",
                //"~/Scripts/jquery-validate/additional-methods.min.js",
                "~/Scripts/bootstrap.bundle.min.js",
                "~/Scripts/respond.js",
                "~/Scripts/moment-with-locales.js",
                "~/Scripts/FontAwesome/all.js",
                "~/Scripts/typeahead.js/typeahead.bundle.min.js",
                "~/Scripts/select2/select2.min.js",
                "~/Scripts/underscore-min.js"
                //"~/Scripts/bootstrap4-editable/js/bootstrap-editable.min.js"   
                 ));
            bundles.Add(new ScriptBundle("~/bundles/DataTable").Include(
                "~/Scripts/DataTables/jquery.dataTables.js",
                "~/Scripts/DataTables/dataTables.bootstrap4.js",
                "~/Scripts/DataTables/dataRender/datetime.js"
            ));
            
            bundles.Add(new ScriptBundle("~/bundles/DataTablePlugin").Include(
                "~/Scripts/DataTables/dataTables.buttons.js",
                "~/Scripts/DataTables/buttons.bootstrap4.js",
                "~/Scripts/DataTables/JSZip-2.5.0/jszip.js",
                "~/Scripts/DataTables/pdfmake-0.1.36/pdfmake.js",
                "~/Scripts/DataTables/pdfmake-0.1.36/vfs_fonts.js",
                "~/Scripts/DataTables/buttons.flash.min.js",
                "~/Scripts/DataTables/buttons.print.min.js",
                "~/Scripts/DataTables/buttons.html5.min.js"
                ));            
            
            bundles.Add(new ScriptBundle("~/bundles/Chart").Include(
                "~/Scripts/Chart.js/Chart.min.js",
                "~/Scripts/Chart.js/chartjs-plugin-datalabels.min.js"
                ));

            //"~/Scripts/FontAwesome/fontawesome.js",
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/DataTables/css/dataTables.bootstrap4.css",
                      "~/Content/DataTables/css/buttons.bootstrap4.css",
                      "~/Content/sitecss/site.css",
                      "~/Content/sitecss/ThanhDieuHuong.css",
                      "~/Content/sitecss/BaiVietStyles.css",
                      "~/Content/JTLine/css/style.css",
                      "~/Content/typeahead/typeahead.css",
                      "~/Content/select2/select2.min.css",
                      "~/Content/bootstrap4-editable/css/bootstrap-editable.css" 
                ));                                           
        }
    }
}
