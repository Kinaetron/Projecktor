using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Projecktor.WebUI
{
    public class SubdomainRoute : RouteBase
    {
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            if(httpContext.Request == null || httpContext.Request.Url == null) {
                return null;
            }

            var host = httpContext.Request.Url.Host;
            var index = host.IndexOf(".");
            string[] segments = httpContext.Request.Url.PathAndQuery.TrimStart('/').Split('/');

            if (index < 0) {
                return null;
            }

            var subdomain = host.Substring(0, index);
            string[] blacklist = { "www", "projecktor", "mail" };

            if(blacklist.Contains(subdomain) == true) {
                return null;
            }

            string controller = "Home";
            string action = (segments.Length > 0 && segments[0] != "") ? segments[0] : "Index";


            string id = (segments.Length > 1 && segments[0] != "") ? segments[1] : "0";

            var routeData = new RouteData(this, new MvcRouteHandler());
            routeData.Values.Add("controller", controller);
            routeData.Values.Add("action", action);
            routeData.Values.Add("subdomain", subdomain);
            routeData.Values.Add("id", id);
            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values) {
            return null;
        }
    }
}