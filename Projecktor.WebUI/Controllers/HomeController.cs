using Projecktor.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projecktor.WebUI.Controllers
{
    public class HomeController : ProjecktorControllerBase
    {
        public HomeController() : base() { }

        // GET: Home
        public ActionResult Index(string subdomain)
        {
            if(Security.IsAuthenticated == false && subdomain == null) {
                return View("Register", new RegisterViewModel());
            }

            var user = Users.GetAllFor(subdomain);

            if (user == null) {
                return new HttpNotFoundResult();
            }

            var userPosts = Posts.GetPostsFor(user.Id).ToList();

            return View("UserPage", userPosts);
        }

        public ActionResult Likes(string subdomain)
        {
            if (Security.IsAuthenticated == false && subdomain == null) {
                return View("Register", new RegisterViewModel());
            }

            var user = Users.GetAllFor(subdomain);
            var likeLine = UserLikes.GetLikesFor(user.Id).ToArray();

            return View("UserPage", likeLine);

            throw new NotImplementedException();
        }

        public ActionResult Post(string subdomain, int id)
        {
            if (Security.IsAuthenticated == false && subdomain == null) {
                return View("Register", new RegisterViewModel());
            }

           var user = Users.GetAllFor(subdomain);

            if (user == null) {
                return new HttpNotFoundResult();
            }

            var post = Posts.GetPost(id);

            if(user.Id != post.Author.Id) {
                return new HttpNotFoundResult();
            }

            return View("Post", post);
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

            return RedirectToAction("Index", "Dashboard");
        }
    }
}