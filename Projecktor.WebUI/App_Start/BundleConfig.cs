using System.Web.Optimization;

namespace Projecktor.WebUI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            string version = System.Reflection.Assembly.GetAssembly(typeof(Controllers.HomeController)).GetName().Version.ToString();
            string cdnUrl = "http://projecktor.azureedge.net/{0}?v=" + version;

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/*.css"));

            bundles.Add(new ScriptBundle("~/bundles/clientfeaturesscripts", string.Format(cdnUrl, "/bundles/clientfeaturesscripts"))
                { CdnFallbackExpression ="window.jquery" }
                .Include(
                "~/Scripts/jquery-3.0.0.min.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Scripts/jquery-ui-1.12.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/galleryscripts", string.Format(cdnUrl, "/bundles/galleryscripts"))
            { CdnFallbackExpression = "$.gallery" }
                .Include(
                "~/Scripts/scrollup.js",
                "~/Scripts/imageGalleryScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/userpagescripts", string.Format(cdnUrl, "/bundles/userpagescripts"))
            { CdnFallbackExpression = "$.infinite" }
                .Include(
               "~/Scripts/infiniteScrollingScript.js",
               "~/Scripts/infinteScrollingUserpage.js"));

            bundles.Add(new ScriptBundle("~/bundles/likespagescripts", string.Format(cdnUrl, "/bundles/likespagescripts"))
            { CdnFallbackExpression = "$.likes" }
                .Include(
               "~/Scripts/infiniteScrollingScript.js",
               "~/Scripts/infiniteScrollingUserLikes.js"));

            bundles.Add(new ScriptBundle("~/bundles/likesdashboardscripts", string.Format(cdnUrl, "/bundles/likesdashboardscripts"))
            { CdnFallbackExpression = "$.action" }
                .Include(
               "~/Scripts/actionScripts.js",
               "~/Scripts/infiniteScrollingScript.js",
               "~/Scripts/infintiteScrollingDashboardLikes.js"));

            bundles.Add(new ScriptBundle("~/bundles/dashboardscripts", string.Format(cdnUrl, "/bundles/dashboardscripts"))
            { CdnFallbackExpression = "$.dashboard" }
                .Include(
                "~/Scripts/actionScripts.js",
                "~/Scripts/infiniteScrollingScript.js",
                "~/Scripts/infiniteScrollingDashboard.js",
                "~/Scripts/imagepostScript.js",
                "~/Scripts/textpostScript.js",
                "~/Scripts/postValidation.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/followscripts")
            { CdnFallbackExpression = "$.follow" }
                .Include(
                 "~/Scripts/actionScripts.js"
                ));

            BundleTable.EnableOptimizations = true;
        }
    }
}