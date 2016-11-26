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
                "~/Scripts/jquery-3.0.0.min.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Scripts/jquery-ui-1.12.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/galleryscripts").Include(
                "~/Scripts/scrollup.js",
                "~/Scripts/imageGalleryScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/galleryscripts").Include(
               "~/Scripts/scrollup.js",
               "~/Scripts/imageGalleryScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/userpagescripts").Include(
               "~/Scripts/infiniteScrollingScript.js",
               "~/Scripts/infinteScrollingUserpage.js"));

            bundles.Add(new ScriptBundle("~/bundles/likespagescripts").Include(
               "~/Scripts/infiniteScrollingScript.js",
               "~/Scripts/infiniteScrollingUserLikes.js"));

            bundles.Add(new ScriptBundle("~/bundles/dashboardscripts").Include(
                "~/Scripts/actionScripts.js",
                "~/Scripts/infiniteScrollingScript.js",
                "~/Scripts/infiniteScrollingDashboard.js",
                "~/Scripts/imagepostScript.js",
                "~/Scripts/textpostScript.js",
                "~/Scripts/postValidation.js"
                ));
        }
    }
}