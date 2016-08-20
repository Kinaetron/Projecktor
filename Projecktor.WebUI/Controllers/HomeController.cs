using System;
using System.Linq;
using System.Web.Mvc;

using Projecktor.WebUI.Models;

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
        }

        public ActionResult Tagged(string id)
        {
            var taggedPosts = Posts.GetTagged(id);

            return View("UserPage", taggedPosts);
        }

        public ActionResult Post(string subdomain, string id)
        {
            if (Security.IsAuthenticated == false && subdomain == null) {
                return View("Register", new RegisterViewModel());
            }

           var user = Users.GetAllFor(subdomain);

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
            var notes = Posts.Notes(int.Parse(id));

            return View("Notes", notes);
        }


        public ActionResult Search(string id)
        {
            SearchModel model = new SearchModel()
            {
                FoundPosts = Posts.GetTagged(id),
                FoundUsers = Users.SearchFor(id)
            };

            return View("SearchPage", model);
        }

        public ActionResult Image(string path)
        {
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

            return RedirectToAction("Index", "Dashboard");
        }
    }
}