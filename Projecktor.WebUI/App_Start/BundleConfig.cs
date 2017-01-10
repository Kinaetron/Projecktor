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


            bundles.Add(new ScriptBundle("~/bundles/basescripts", string.Format(cdnUrl, "/bundles/basescripts"))
            //{ CdnFallbackExpression = "$.base" }
                .Include(
                "~/Scripts/jquery-3.0.0.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery-ui-1.12.1.js"
                ));


                bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/*.css",
                "~/Content/themes/base/*.css"));


            bundles.Add(new ScriptBundle("~/bundles/galleryscripts", string.Format(cdnUrl, "/bundles/galleryscripts"))
            //{ CdnFallbackExpression = "$.gallery" }
                .Include(
                "~/Scripts/scrollup.js",
                "~/Scripts/imageGalleryScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/userpagescripts", string.Format(cdnUrl, "/bundles/userpagescripts"))
            //{ CdnFallbackExpression = "$.infinite" }
                .Include(
               "~/Scripts/actionScripts.js",
               "~/Scripts/infiniteScrollingScript.js",
               "~/Scripts/infinteScrollingUserpage.js",
               "~/Scripts/notesScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/taggedscripts", string.Format(cdnUrl, "/bundles/taggedscripts"))
            //{ CdnFallbackExpression = "$.tagged" }
               .Include(
              "~/Scripts/actionScripts.js",
              "~/Scripts/infiniteScrollingScript.js",
              "~/Scripts/infinteScrollingTagged.js",
              "~/Scripts/notesScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/taggedscriptsexternal", string.Format(cdnUrl, "/bundles/taggedscriptsexternal"))
            //{ CdnFallbackExpression = "$.taggedexternal" }
               .Include(
              "~/Scripts/actionScripts.js",
              "~/Scripts/infiniteScrollingScript.js",
              "~/Scripts/infiniteScrollingTaggedExternal.js",
              "~/Scripts/notesScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/activityscripts", string.Format(cdnUrl, "/bundles/activityscripts"))
            //{ CdnFallbackExpression = "$.taggedactivity" }
              .Include(
             "~/Scripts/infiniteScrollingActivity.js"));

            bundles.Add(new ScriptBundle("~/bundles/likespagescripts", string.Format(cdnUrl, "/bundles/likespagescripts"))
            //{ CdnFallbackExpression = "$.likes" }
                .Include(
               "~/Scripts/infiniteScrollingScript.js",
               "~/Scripts/infiniteScrollingUserLikes.js",
               "~/Scripts/notesScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/likesdashboardscripts", string.Format(cdnUrl, "/bundles/likesdashboardscripts"))
            //{ CdnFallbackExpression = "$.action" }
                .Include(
               "~/Scripts/actionScripts.js",
               "~/Scripts/infiniteScrollingScript.js",
               "~/Scripts/infintiteScrollingDashboardLikes.js",
               "~/Scripts/notesScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/dashboardscripts", string.Format(cdnUrl, "/bundles/dashboardscripts"))
            //{ CdnFallbackExpression = "$.dashboard" }
                .Include(
                "~/Scripts/actionScripts.js",
                "~/Scripts/infiniteScrollingScript.js",
                "~/Scripts/infiniteScrollingDashboard.js",
                "~/Scripts/imagepostScript.js",
                "~/Scripts/textpostScript.js",
                "~/Scripts/postValidation.js",
                "~/Scripts/notesScript.js"
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