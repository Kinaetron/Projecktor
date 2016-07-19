using System;
using System.Linq;
using System.Web.Mvc;
using Projecktor.WebUI.Models;
using Projecktor.Domain.Entites;

namespace Projecktor.WebUI.Controllers
{
    public class DashboardController : ProjecktorControllerBase
    {
        // GET: Home
        public ActionResult Index()
        {
            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            var timeline = TextPosts.GetTimeLineFor(Security.UserId).ToArray();

            return View("Dashboard", timeline);
        }

        public ActionResult Likes()
        {
            if(Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            var likeLine = UserLikes.GetLikesFor(Security.UserId).ToArray();

            return View("Likes", likeLine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Follow(string username)
        {
            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            Users.Follow(username, Security.GetCurrentUser());

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public JsonResult Reblog(int postId)
        {
            UserReblogs.Reblog(CurrentUser.Id, postId);
            return Json(new { msg = "Successful" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnFollow(string username)
        {
            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            Users.Unfollow(username, Security.GetCurrentUser());

            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult Profiles()
        {
            var users = Users.AllUsers();

            return View(users);
        }

        public ActionResult Followers()
        {
            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            var followers = Users.GetAllFor(Security.UserId).Followers;

            return View("Followers", new FollowViewModel() {
                FollowData = followers
            });
        }

        public ActionResult Following()
        {
            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            var followers = Users.GetAllFor(Security.UserId).Following;

            return View("Following", new FollowViewModel() {
                FollowData = followers
            });
        }

        [HttpGet]
        public ActionResult TextPost() {
            return View("TextPost", new CreateTextPostViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TextPost(CreateTextPostViewModel model)
        {
            if(ModelState.IsValid == true) {
                TextPosts.Create(Security.UserId, model.TextPost);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public JsonResult DeletePost(int postId)
        {
            TextPosts.Delete(postId);
            return Json(new { msg = "Successful" });
        }

        [HttpPost]
        public JsonResult Like(int postId)
        {
            if(ModelState.IsValid == true) {
                UserLikes.Like(Security.UserId, postId);
            }

            return Json(new { msg = "Successful" });
        }

        [HttpPost]
        public JsonResult Unlike(int postId)
        {
            if (ModelState.IsValid == true) {
                UserLikes.Unlike(CurrentUser.Likes.FirstOrDefault(u => u.PostId == postId));
            }

            return Json(new { msg = "Successful" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Security.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}