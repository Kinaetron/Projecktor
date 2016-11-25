using System.Web.Optimization;

namespace Projecktor.WebUI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/clientfeaturesscripts").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/jquery-3.0.0.js",
                "~/Scripts/jquery-3.0.0.min.js",
                "~/Scripts/jquery-3.0.0.slim.js",
                "~/Scripts/jquery-3.0.0.intellisense.js",
                "~/Scripts/jquery-3.0.0.slim.min.js",
                "~/Scripts/jquery-ui-1.12.1.js",
                "~/Scripts/jquery.validate-vsdoc.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Scripts/modernizr-2.6.2.js",
                "~/Scripts/jquery-ui-1.12.1.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/customscripts").Include(
            //     "~/Scripts/actionScripts.js",
            //     "~/Scripts/imageGalleryScript.js",
            //     "~/Scripts/imagepostScript.js",
            //     "~/Scripts/infiniteScrollingDashboard.js",
            //     "~/Scripts/infiniteScrollingScript.js",
            //     "~/Scripts/infiniteScrollingUserLikes.js",
            //     "~/Scripts/infinteScrollingUserpage.js",
            //     "~/Scripts/infintiteScrollingDashboardLikes.js",
            //     "~/Scripts/postValidation.js",
            //     "~/Scripts/scrollup.js",
            //     "~/Scripts/searchScript.js",
            //     "~/Scripts/textpostScript.js"
            //     ));
        }
    }
}