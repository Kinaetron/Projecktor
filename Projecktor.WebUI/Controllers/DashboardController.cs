using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using Projecktor.WebUI.Models;
using Projecktor.Domain.Entites;
using System.Collections.Generic;

namespace Projecktor.WebUI.Controllers
{
    public class DashboardController : ProjecktorControllerBase
    {
        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["loggedIn"];

            if (Security.IsAuthenticated == false && cookie == null) {
                return RedirectToAction("Index", "Home");
            }

            if(Security.IsAuthenticated == false && cookie != null) {
                Security.Login(cookie.Value);
            }

            IEnumerable<PostViewModel> timeline = Posts.GetTimeLineFor(Security.UserId).Take(10).ToArray();
            return View("Dashboard", timeline);
        }


        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public ActionResult Likes()
        {
            if(Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<PostViewModel> likeLine = UserLikes.GetLikesFor(Security.UserId).Take(10).ToArray();
            return View("Likes", likeLine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public ActionResult Follow(string username)
        {
            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            Users.Follow(username, Security.GetCurrentUser());
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public JsonResult Reblog(int textId, int reblogId, int sourceId)
        {
            Post madeReblog = Posts.Reblog(CurrentUser.Id, textId, reblogId, sourceId);
            Hashtag[] tags = Hashtags.GetHashTagsFor(sourceId).ToArray();

            if (tags != null) {
                Hashtags.Create(madeReblog.Id, Security.UserId ,tags);
            }

            return Json(new { msg = "Successful" });
        }

        [HttpPost]
        public JsonResult DeleteReblog(int postId)
        {
            Posts.DeleteReblog(postId);
            Hashtags.Delete(postId);
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

        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public ActionResult Profiles()
        {
            IEnumerable<User> users = Users.AllUsers();
            return View(users);
        }

        [HttpGet]
        public JsonResult GetPosts(int pageIndex, int pageSize)
        {
            List<int> postIds = new List<int>();
            var timeline = Posts.GetTimeLineFor(Security.UserId).Skip(pageIndex * pageSize).Take(pageSize).ToArray();

            foreach (PostViewModel item in timeline) {
                postIds.Add(item.PostId);
            }

            return Json(postIds.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLikes(int pageIndex, int pageSize)
        {
            List<int> postIds = new List<int>();
            IEnumerable<PostViewModel> likeLine = UserLikes.GetLikesFor(Security.UserId).Skip(pageIndex * pageSize).Take(pageSize).ToArray();

            foreach (PostViewModel item in likeLine) {
                postIds.Add(item.PostId);
            }

            return Json(postIds.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowPost(List<int> data)
        {
            List<PostViewModel> models = new List<PostViewModel>();

            foreach (var number in data) {
                models.Add(Posts.GetPost(number));
            }

            return PartialView("_MultiPosts", models);
        }

        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public ActionResult Followers()
        {
            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<User> followers = Users.GetFollowers(CurrentUser.Id);

            return View("Followers", new FollowViewModel() {
                FollowData = followers
            });
        }

        public ActionResult Following()
        {
            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<User> followers = Users.GetFollowing(CurrentUser.Id);

            return View("Following", new FollowViewModel() {
                FollowData = followers
            });
        }
  
        [HttpGet]
        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public ActionResult TextPost() {
            return View("TextPost", new CreateTextPostViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public ActionResult TextPost(CreateTextPostViewModel model)
        {
            if(ModelState.IsValid == true)
            {
                Post made = Posts.CreateTextPost(Security.UserId, model.TextPost);

                if(model.Hashtags != null) {
                    Hashtags.Create(made.Id, Security.UserId, model.Hashtags);
                }
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public ActionResult ImagePost() {
            return View("ImagePost", new CreateImagePostViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public ActionResult ImagePost(CreateImagePostViewModel model)
        {
            string[] imagePath = new string[6];
            HttpPostedFileBase[] images = model.Images.ToArray();

            int count = (model.Images.Count() > 6) ? 6 : model.Images.Count();

            for (int i = 0; i < count; i++)
            {

                string imageName = "projecktor_" + GetRandomString();

                string imageNameOriginal = imageName + Path.GetExtension(images[i].FileName);
                string imageName720p = imageName + "_720" + Path.GetExtension(images[i].FileName);
                string imageName540 = imageName + "_540" + Path.GetExtension(images[i].FileName);

                string folderName = GetRandomString();
                string filePathOriginal = "~/" + folderName + "/" + imageNameOriginal;
                string filePath720p = "~/" + folderName + "/" + imageName720p;
                string filePath540 = "~/" + folderName + "/" + imageName540;

                Directory.CreateDirectory(Server.MapPath(folderName));
                string physicalPath = Server.MapPath(filePathOriginal);

                Bitmap Image720p = ResizeImage720p(Image.FromStream(images[i].InputStream, true, true));
                Image720p.Save(Server.MapPath(filePath720p));

                Bitmap Image540 = ResizeImage540(Image.FromStream(images[i].InputStream, true, true));
                Image540.Save(Server.MapPath(filePath540));

                images[i].SaveAs(physicalPath);
                imagePath[i] = filePathOriginal;
            }

            Post made = Posts.CreateImagePost(Security.UserId, model.Text, imagePath);

            if(model.Hashtags != null) {
                Hashtags.Create(made.Id, Security.UserId, model.Hashtags);
            }

            return RedirectToAction("Index", "Dashboard");
        }


        [HttpPost]
        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public JsonResult DeletePost(int postId)
        {
            Post post = Posts.Getby(postId);

            if(post.Image1 != null)
            {
                System.IO.File.Delete(Server.MapPath(post.Image1));

                string[] image = post.Image1.Split('.');
                System.IO.File.Delete(Server.MapPath(image[0] + "_540." + image[1]));
                System.IO.File.Delete(Server.MapPath(image[0] + "_720." + image[1]));
            }
            if (post.Image2 != null)
            {
                System.IO.File.Delete(Server.MapPath(post.Image2));

                string[] image = post.Image2.Split('.');
                System.IO.File.Delete(Server.MapPath(image[0] + "_540." + image[1]));
                System.IO.File.Delete(Server.MapPath(image[0] + "_720." + image[1]));
            }
            if (post.Image3 != null)
            {
                System.IO.File.Delete(Server.MapPath(post.Image3));

                string[] image = post.Image3.Split('.');
                System.IO.File.Delete(Server.MapPath(image[0] + "_540." + image[1]));
                System.IO.File.Delete(Server.MapPath(image[0] + "_720." + image[1]));
            }
            if (post.Image4 != null)
            {
                System.IO.File.Delete(Server.MapPath(post.Image4));

                string[] image = post.Image4.Split('.');
                System.IO.File.Delete(Server.MapPath(image[0] + "_540." + image[1]));
                System.IO.File.Delete(Server.MapPath(image[0] + "_720." + image[1]));
            }
            if (post.Image5 != null)
            {
                System.IO.File.Delete(Server.MapPath(post.Image5));

                string[] image = post.Image5.Split('.');
                System.IO.File.Delete(Server.MapPath(image[0] + "_540." + image[1]));
                System.IO.File.Delete(Server.MapPath(image[0] + "_720." + image[1]));
            }
            if (post.Image6 != null)
            {
                System.IO.File.Delete(post.Image6);

                string[] image = post.Image6.Split('.');
                System.IO.File.Delete(Server.MapPath(image[0] + "_540." + image[1]));
                System.IO.File.Delete(Server.MapPath(image[0] + "_720." + image[1]));
            }

            Posts.Delete(postId);
            Hashtags.Delete(postId);
            return Json(new { msg = "Successful" });
        }

        [HttpPost]
        public JsonResult Like(int postId, int sourceId)
        {
            if(ModelState.IsValid == true) {
                UserLikes.Like(Security.UserId, postId, sourceId);
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

        public ActionResult Search(string id)
        {
            SearchModel model = new SearchModel()
            {
                FoundPosts = Posts.GetTagged(id),
                FoundUsers = Users.SearchFor(id)
            };

            return View("SearchPage", model);
        }

        [HttpGet]
        public JsonResult AutoComplete(string id)
        {
            List<string> results = new List<string>();

            results.AddRange(Users.SearchUsername(id));
            results.AddRange(Posts.GetTagTerms(id));

            return Json(results.OrderBy(q => q).Distinct().Take(5), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public ActionResult Settings()
        {
            User user = Users.GetBy(Security.UserId);
            SettingsViewModel settings = new SettingsViewModel()
            {
                Username = user.Username,
                Email = user.Email
            };

            return View("Settings", settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public ActionResult Settings(SettingsViewModel settings)
        {

            if (Security.Authenticate(CurrentUser.Username, settings.CurrentPassword) == false)
            {
                ModelState.AddModelError("Username", "The password was incorrect");
                return View("Settings", settings);
            }

            Users.Settings(settings.Username, settings.NewPassword, settings.Email, Security.UserId);
            return View("Settings", settings);
        }

        [HttpGet]
        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public ActionResult Activity()
        {
            List<ActivityViewModel> Activity = Users.Activity(CurrentUser.Id).ToList();
            return View("Activity", Activity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public ActionResult Logout()
        {
            if(Request.Cookies["loggedIn"] != null)
            {
                string name = Request.Cookies["loggedIn"].Name;

                HttpCookie cookie = new HttpCookie(name);
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Domain = ".projecktor.com";
                Response.Cookies.Add(cookie);

            }
            Security.Logout();
            return RedirectToAction("Index", "Home");
        }

        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
        }

        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public static Bitmap ResizeImage540(Image image)
        {
            if (image.Width <= 540 && image.Height <= 810) {
                return (Bitmap)image;
            }

            double aspectRatio = (double)image.Width / (double)image.Height;
            double boxRatio = (double)540 / (double)810;
            double scaleFactor = 0;

            if(boxRatio > aspectRatio) {
                scaleFactor = (double)810 / (double)image.Height;
            }
            else {
                scaleFactor = (double)540 / (double)image.Width;
            }

            double newWidth = (double)image.Width * scaleFactor;
            double newHeight = (double)image.Height * scaleFactor;


            return ResizeImage(image, (int)newWidth, (int)newHeight);
        }

        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public static Bitmap ResizeImage720p(Image image)
        {
            if(image.Width <= 1280 && image.Height <= 720) {
                return (Bitmap)image;
            }

            double aspectRatio = (double)image.Width / (double)image.Height;
            double boxRatio = (double)1280 / (double)720;
            double scaleFactor = 0;

            if (boxRatio > aspectRatio) {
                scaleFactor = (double)720 / (double)image.Height;
            }
            else {
                scaleFactor = (double)1280 / (double)image.Width;
            }

            double newWidth = image.Width * scaleFactor;
            double newHeight = image.Height * scaleFactor;


            return ResizeImage(image, (int)newWidth, (int)newHeight);
        }

        [OutputCache(Duration = 8600, VaryByParam = "none")]
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            Rectangle destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (ImageAttributes wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}