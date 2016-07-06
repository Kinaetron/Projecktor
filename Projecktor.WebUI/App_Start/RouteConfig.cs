using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Projecktor.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
             routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add(new SubdomainRoute());
            //routes.MapRoute(
            //       name: "Followers",
            //       url: "followers",
            //       defaults: new { controller = "home", action = "followers" });

            //routes.MapRoute(
            //       name: "Following",
            //       url: "following",
            //       defaults: new { controller = "home", action = "following" });

            //routes.MapRoute(
            //      name: "Dashboard",
            //      url: "dashboard",
            //      defaults: new { controller = "home", action = "dashboard" });

            //favorites

            //routes.MapRoute(
            //        name: "Default",
            //        url: "",
            //        defaults: new { controller = "home", action = "index" });

            routes.MapRoute(
                   name: "Default",
                   url: "{controller}/{action}/{id}",
                   defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
