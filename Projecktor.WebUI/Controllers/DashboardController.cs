using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Projecktor.WebUI.Models;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Follow()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnFollow(string username)
        {
            throw new NotImplementedException();
        }

        public ActionResult Followers(string username)
        {
            throw new NotImplementedException();
        }

        public ActionResult Following()
        {
            throw new NotImplementedException();
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
    }
}