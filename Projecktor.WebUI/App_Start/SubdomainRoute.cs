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
            if (httpContext.Request == null || httpContext.Request.Url == null) {
                return null;
            }

            string host = httpContext.Request.Url.Host;
            int index = host.IndexOf(".");
            string[] segments = httpContext.Request.Url.PathAndQuery.TrimStart('/').Split('/');

            if (index < 0) {
                return null;
            }

            string  subdomain = host.Substring(0, index);
            string[] blacklist = { "www", "projecktor", "mail" };

            if(blacklist.Contains(subdomain) == true) {
                return null;
            }

            string controller = "Home";
            string action = (segments.Length > 0 && segments[0] != "") ? segments[0] : "Index";

            if (action.Contains("?") == true) {
                string[] segs = action.TrimStart('?').Split('?');
                action = segs[0];
            }

            string id = "0";
            if(segments[0] != "") {
                if(segments.Length > 1) {
                    id = segments[1];
                }
                else if(segments[0].Contains("=") == true) {
                    string[] segs = segments[0].TrimStart('=').Split('=');
                    id = segs[1];
                }
            }

            RouteData routeData = new RouteData(this, new MvcRouteHandler());
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