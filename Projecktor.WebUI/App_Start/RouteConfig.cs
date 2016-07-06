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

            routes.MapRoute(
                   name: "Followers",
                   url:  "followers",
                   defaults: new { controller = "dashboard", action = "followers" });

            routes.MapRoute(
                   name: "Following",
                   url:  "following",
                   defaults: new { controller = "dashboard", action = "following" });

            routes.MapRoute(
                name: "Follow",
                url:  "follow",
                defaults: new { controller = "dashboard", action = "follow" });

            routes.MapRoute(
                name: "Unfollow",
                url:  "unfollow",
                defaults: new { controller = "dashboard", action = "unfollow" });

            routes.MapRoute(
                  name: "Dashboard",
                  url:  "dashboard",
                  defaults: new { controller = "dashboard", action = "index" });

            //favorites

            routes.MapRoute(
                    name: "Default",
                    url: "{action}",
                    defaults: new { controller = "home", action = "index" });

        }
    }
}
