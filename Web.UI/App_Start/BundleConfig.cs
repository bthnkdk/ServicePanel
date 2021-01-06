using System.Web.Optimization;

namespace Web.UI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region vendor

            //bundles.Add(new ScriptBundle("~/vendor/js").Include(
            //            "~/vendor/bootstrap/js/bootstrap.bundle.min.js",
            //            "~/vendor/jquery-easing/jquery.easing.min.js",
            //            "~/vendor/jquery-mask/jquery.mask.min.js",
            //            "~/vendor/sammy/min/sammy-latest.min.js",
            //            "~/vendor/fullcalendar/fullcalendar.bundle.js",
            //            "~/vendor/admin/js/sb-admin.js")
            //            );

            #endregion

            #region core


            bundles.Add(new ScriptBundle("~/Scripts/corejs").Include(
                    "~/Scripts/sammy/min/sammy-latest.min.js",
                    "~/Scripts/webui.js",
                    "~/Scripts/coreUtils.js",
                    "~/Scripts/layoutrouting.js"
                    ));

            //bundles.Add(new StyleBundle("~/Content/style").Include(
            //        "~/Content/site.css"));

            #endregion

            #region Awesome 
            bundles.Add(new ScriptBundle("~/Scripts/awe/script").Include(
                    "~/Scripts/awe/AwesomeMvc.js",
                    "~/Scripts/awe/awem.js",
                    "~/Scripts/awe/utils.js")
                    );
            bundles.Add(new StyleBundle("~/Content/awe/themes/wui/styles").Include(
                    "~/Content/awe/themes/wui/AwesomeMvc.css"));
            #endregion

            //#region Login

            //bundles.Add(new ScriptBundle("~/Scripts/login").Include(
            //        "~/Scripts/login.js"
            //        ));

            //bundles.Add(new StyleBundle("~/Content/login").Include(
            //        "~/Content/login.css"
            //        ));

            //#endregion

            //BundleTable.EnableOptimizations = true;
        }

    }
}