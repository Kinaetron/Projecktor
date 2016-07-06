using System;
using System.Collections.Generic;
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

            string controller = (segments.Length > 0) ? segments[0] : "Home";
            string action = (segments.Length > 1) ? segments[1] : "Index";

            var routeData = new RouteData(this, new MvcRouteHandler());
            routeData.Values.Add("controller", controller); //Goes to the relevant Controller class
            routeData.Values.Add("action", action); //Goes to the relevant action method on the specified Controller
            routeData.Values.Add("subdomain", subdomain); // pass subdomain as argument to action method
            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            //Implement your formatting Url formating here
            return null;
        }
    }
}