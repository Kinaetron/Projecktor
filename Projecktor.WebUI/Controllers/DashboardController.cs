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

        public ActionResult UserLikes()
        {
            if(Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            var likeLine = Likes.GetLikesFor(Security.UserId).ToArray();
            return View("Dashboard", likeLine);
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
        [ValidateAntiForgeryToken]
        public ActionResult Like(TextPost post)
        {
            if(ModelState.IsValid == true) {
                Likes.Like(Security.UserId, post.Id);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unlike(TextPost post)
        {
            if (ModelState.IsValid == true) {
                Likes.Unlike(CurrentUser.Likes.FirstOrDefault(u => u.PostId == post.Id));
            }

            return RedirectToAction("Index", "Dashboard");
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