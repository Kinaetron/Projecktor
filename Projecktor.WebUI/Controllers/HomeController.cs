using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using Projecktor.WebUI.Models;
using Projecktor.Domain.Entites;

namespace Projecktor.WebUI.Controllers
{

    public class HomeController : ProjecktorControllerBase
    {
        public HomeController() : base() { }

        public ActionResult Index(string subdomain)
        {
            HttpCookie cookie = Request.Cookies["loggedIn"];

            if(Security.IsAuthenticated == false && subdomain == null)
            {
                if(cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddYears(1);
                    cookie.Domain = ".projecktor.com";
                    Response.Cookies.Add(cookie);

                    Security.Login(cookie.Value);
                    return RedirectToAction("Index", "Dashboard");
                }

                return View("Register", new RegisterViewModel());
            }


            if(Security.IsAuthenticated == true && subdomain == null) {
                return RedirectToAction("Index", "Dashboard");
            }

            if(Security.IsAuthenticated == false && subdomain != null) {
                if (cookie != null) {
                    Security.Login(cookie.Value);
                }
            }

            User user = Users.GetAllFor(subdomain);

            if (user == null) {
                return new HttpNotFoundResult();
            }

            var userPosts = Posts.GetPostsFor(user.Id).Take(10).ToList();
            return View("UserPage", userPosts);
        }

        [HttpGet]
        public JsonResult GetUserPosts(string subdomain, int pageIndex, int pageSize)
        {
            User user = Users.GetAllFor(subdomain);

            List<int> postIds = new List<int>();
            var timeline = Posts.GetPostsFor(user.Id).Skip(pageIndex * pageSize).Take(pageSize).ToArray();

            foreach (PostViewModel item in timeline) {
                postIds.Add(item.PostId);
            }

            return Json(postIds.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Likes(string subdomain)
        {
            if (Security.IsAuthenticated == false && subdomain == null) {
                return View("Register", new RegisterViewModel());
            }

            User user = Users.GetAllFor(subdomain);
            IEnumerable<PostViewModel> likeLine = UserLikes.GetLikesFor(user.Id).Take(10).ToArray();
            return View("Likes", likeLine);
        }

        [HttpGet]
        public JsonResult GetUserLikes(string subdomain, int pageIndex, int pageSize)
        {
            User user = Users.GetAllFor(subdomain);

            List<int> postIds = new List<int>();
            IEnumerable<PostViewModel> likeLine = UserLikes.GetLikesFor(user.Id).Skip(pageIndex * pageSize).Take(pageSize).ToArray();

            foreach (PostViewModel item in likeLine) {
                postIds.Add(item.PostId);
            }

            return Json(postIds.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gallery(string id)
        {
            Post post = Posts.Getby(int.Parse(id));
            List<string> images = new List<string>();

            string[] image;

            if (post.Image1 != null)
            {
                image = post.Image1.Split('.');
                images.Add(Url.Content(image[0] + "_720." + image[1]));
            }
            if (post.Image2 != null)
            {
                image = post.Image2.Split('.');
                images.Add(Url.Content(image[0] + "_720." + image[1]));
            }
            if (post.Image3 != null)
            {
                image = post.Image3.Split('.');
                images.Add(Url.Content(image[0] + "_720." + image[1]));
            }
            if (post.Image4 != null)
            {
                image = post.Image4.Split('.');
                images.Add(Url.Content(image[0] + "_720." + image[1]));
            }
            if (post.Image5 != null)
            {
                image = post.Image5.Split('.');
                images.Add(Url.Content(image[0] + "_720." + image[1]));
            }
            if (post.Image6 != null)
            {
                image = post.Image6.Split('.');
                images.Add(Url.Content(image[0] + "_720." + image[1]));
            }

            return Json(images, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowUserPost(List<int> data)
        {
            List<PostViewModel> models = new List<PostViewModel>();

            foreach (var number in data) {
                models.Add(Posts.GetPost(number));
            }

            return PartialView("_MultiPosts", models);
        }

        public ActionResult Tagged(string subdomain, string id)
        {
            IEnumerable<PostViewModel> taggedPosts;

            if(subdomain == null) {
                taggedPosts = Posts.GetTagged(id);
            }
            else {
                taggedPosts = Posts.GetTaggedUser(id, subdomain);
            }

            return View("UserPage", taggedPosts);
        }

        public ActionResult Post(string subdomain, string id)
        {
            if (Security.IsAuthenticated == false && subdomain == null) {
                return View("Register", new RegisterViewModel());
            }

           User user = Users.GetAllFor(subdomain);

            if (user == null) {
                return new HttpNotFoundResult();
            }

            var post = Posts.GetPost(int.Parse(id));

            if(user.Id != post.Author.Id) {
                return new HttpNotFoundResult();
            }
            return View("Post", post);
        }

        public ActionResult Notes(string id)
        {
            IEnumerable<Note> notes = Posts.Notes(int.Parse(id));
            return View("Notes", notes);
        }

        public ActionResult Image(string path) {
            return File(path, "image");
        }

        [HttpGet]
        public ActionResult Register() {
            return View("Register", new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (Security.IsAuthenticated == true) {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid == false) {
                return View("Register", model);
            }

            if (Security.DoesUserExist(model.Username) == true)
            {
                ModelState.AddModelError("Username", "Username is already taken");
                return View("Register", model);
            }

            Security.CreateUser(model);
            return View("Login", new LoginViewModel());
        }

        [HttpGet]
        public ActionResult Login() {
            return View("Login", new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (Security.IsAuthenticated == true) {
                return RedirectToAction("Index", "Dashboard");
            }

            if (ModelState.IsValid == false) {
                return View("Login", model);
            }

            if (Security.Authenticate(model.Username, model.Password) == false)
            {
                ModelState.AddModelError("Username", "Username and/or password was incorrect");
                return View("Login", model);
            }

            Security.Login(model.Username);

            HttpCookie cookie = new HttpCookie("loggedIn", Security.GetCurrentUser().Username);
            cookie.Domain = ".projecktor.com";
            cookie.Expires = DateTime.Now.AddYears(1);
            Response.AppendCookie(cookie);

            return RedirectToAction("Index", "Dashboard");
        }
    }
}