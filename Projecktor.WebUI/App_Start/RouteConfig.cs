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
                 name: "TextPost",
                 url: "textpost",
                 defaults: new { controller = "dashboard", action = "textpost" });

            routes.MapRoute(
              name: "Like",
              url: "like",
              defaults: new { controller = "dashboard", action = "like" });

            routes.MapRoute(
             name: "Unlike",
             url: "unlike",
             defaults: new { controller = "dashboard", action = "unlike" });

            routes.MapRoute(
                   name: "Followers",
                   url: "followers",
                   defaults: new { controller = "dashboard", action = "followers" });

            routes.MapRoute(
                   name: "Following",
                   url:  "following",
                   defaults: new { controller = "dashboard", action = "following" });

            routes.MapRoute(
                   name: "Profiles",
                   url: "profiles",
                   defaults: new { controller = "dashboard", action = "profiles" });

            routes.MapRoute(
                name: "Follow",
                url:  "follow",
                defaults: new { controller = "dashboard", action = "follow" });

            routes.MapRoute(
                name: "Unfollow",
                url:  "unfollow",
                defaults: new { controller = "dashboard", action = "unfollow" });

            routes.MapRoute(
              name: "Logout",
              url:  "logout",
              defaults: new { controller = "dashboard", action = "logout" });

            routes.MapRoute(
                  name: "Dashboard",
                  url:  "dashboard",
                  defaults: new { controller = "dashboard", action = "index" });

            routes.MapRoute(
                 name: "UserLikes",
                 url:  "likes",
                 defaults: new { controller = "dashboard", action = "userlikes" });

            routes.MapRoute(
                    name: "Default",
                    url:  "{action}",
                    defaults: new { controller = "home", action = "index" });

        }
    }
}
