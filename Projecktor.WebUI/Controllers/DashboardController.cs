using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projecktor.WebUI.Controllers
{
    public class DashboardController : ProjecktorControllerBase
    {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            throw new NotImplementedException();
        }
    }
}