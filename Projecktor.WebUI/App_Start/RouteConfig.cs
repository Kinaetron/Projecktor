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
                 url:  "textpost",
                 defaults: new { controller = "dashboard", action = "textpost" });

            routes.MapRoute(
                name: "ImagePost",
                url:  "imagepost",
                defaults: new { controller = "dashboard", action = "imagepost" });

            routes.MapRoute(
                name: "Followers",
                url:  "followers",
                defaults: new { controller = "dashboard", action = "followers" });

            routes.MapRoute(
                name: "Following",
                url:  "following",
                defaults: new { controller = "dashboard", action = "following" });

            routes.MapRoute(
                name: "Profiles",
                url:  "profiles",
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
                name: "Likes",
                url:  "likes",
                defaults: new { controller = "dashboard", action = "likes" });

            routes.MapRoute(
                name: "Settings",
                url:  "settings",
                defaults: new { controller = "dashboard", action = "settings" });

            routes.MapRoute(
                name: "Activity",
                url:  "activity",
                defaults: new { controller = "dashboard", action = "activity" });

            routes.MapRoute(
                name: "GetPosts",
                url:  "getposts",
                defaults: new { controller = "dashboard", action = "getposts" });

            routes.MapRoute(
                name: "GetLikes",
                url:  "getlikes",
                defaults: new { controller = "dashboard", action = "getlikes" });

            routes.MapRoute(
                name: "ShowPost",
                url:  "showpost",
                defaults: new { controller = "dashboard", action = "showpost" });

            routes.MapRoute(
                name: "Search",
                url:  "search/{id}",
                defaults: new { controller = "dashboard", action = "search" });

            routes.MapRoute(
               name: "Autocomplete",
               url:  "autocomplete",
               defaults: new { controller = "dashboard", action = "autocomplete" });

            routes.MapRoute(
               name: "Reblog",
               url: "reblog",
               defaults: new { controller = "home", action = "reblog" });

            routes.MapRoute(
                name: "DeleteReblog",
                url: "deletereblog",
                defaults: new { controller = "home", action = "deletereblog" });


            routes.MapRoute(
                name: "DeletePost",
                url: "deletepost",
                defaults: new { controller = "home", action = "deletepost" });

            routes.MapRoute(
             name: "Like",
             url: "like",
             defaults: new { controller = "home", action = "like" });

            routes.MapRoute(
                name: "Unlike",
                url: "unlike",
                defaults: new { controller = "home", action = "unlike" });

            routes.MapRoute(
               name: "Post",
               url:  "post/{id}",
               defaults: new { controller = "home", action = "post" });

            routes.MapRoute(
                name: "Tagged",
                url:  "tagged/{id}",
                defaults: new { controller = "home", action = "tagged" });

            routes.MapRoute(
               name: "GetUserPosts",
               url:  "getuserposts",
               defaults: new { controller = "home", action = "getuserposts" });

            routes.MapRoute(
                name: "GetUserLikes",
                url:  "getuserlikes",
                defaults: new { controller = "home", action = "getuserlikes" });

            routes.MapRoute(
                name: "ShowUserPost",
                url:  "showuserpost",
                defaults: new { controller = "home", action = "showuserpost" });

            routes.MapRoute(
               name: "ForgotPassword",
               url:  "forgotpassword",
               defaults: new { controller = "home", action = "forgotpassword" });

            routes.MapRoute(
             name: "Register",
             url:  "register",
             defaults: new { controller = "home", action = "register" });

            routes.MapRoute(
            name: "Login",
            url:  "login",
            defaults: new { controller = "home", action = "login" });

            routes.MapRoute(
              name: "Gallery",
              url:  "gallery/{id}",
              defaults: new { controller = "home", action = "gallery" });

            routes.MapRoute(
                name: "Image",
                url:  "image",
                defaults: new { controller = "home", action = "image" });

            routes.MapRoute(
                name: "PasswordReset",
                url: "passwordreset/{passwordId}/{userId}",
                defaults: new { controller = "home", action = "passwordreset" });

            routes.MapRoute(
                name: "Default",
                url:  "{action}",
                defaults: new { controller = "home", action = "index" });
        }
    }
}
