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
        }
    }
}