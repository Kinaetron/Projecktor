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

        private static int lastPageIndex = 0;
        private static int lastPageIndexLikes = 0;

        private const int pageSizeInt = 10;

        public ActionResult Index()
        {
            lastPageIndex = 0;
            lastPageIndexLikes = 0;

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

        public JsonResult PostCount()
        {
            List<PostViewModel> posts = Posts.GetTimeLineFor(Security.UserId).ToList();
            posts.RemoveAll(p => p.Author.Id == Security.UserId);

            return Json(posts.Count(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Likes()
        {
            if(Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<PostViewModel> likeLine = UserLikes.GetLikesFor(Security.UserId).Take(10).ToArray();
            return View("Likes", likeLine);
        }

        public ActionResult Profiles()
        {
            IEnumerable<User> users = Users.AllUsers();
            return View(users);
        }

        [HttpGet]
        public JsonResult GetPostsCheck(int pageIndex, int pageSize)
        {
            if (pageIndex * pageSizeInt > Posts.GetTimeLineFor(Security.UserId).Count()) {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetPosts(int pageIndex, int pageSize)
        {
            if (pageIndex * pageSizeInt > Posts.GetTimeLineFor(Security.UserId).Count() || pageIndex == lastPageIndex) {
                return Json(null);
            }

            List<int> postIds = new List<int>();
            var timeline = Posts.GetTimeLineFor(Security.UserId).Skip(pageIndex * pageSizeInt).Take(pageSizeInt).ToArray();

            foreach (PostViewModel item in timeline) {
                postIds.Add(item.PostId);
            }

            lastPageIndex = pageIndex;

            return Json(postIds.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLikesCheck(int pageIndex, int pageSize)
        {
            if (pageIndex * pageSizeInt > UserLikes.GetLikesFor(Security.UserId).Count()) {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetLikes(int pageIndex, int pageSize)
        {
            if (pageIndex * pageSizeInt > UserLikes.GetLikesFor(Security.UserId).Count() || pageIndex == lastPageIndexLikes) {
                return Json(null);
            }

            List<int> postIds = new List<int>();
            IEnumerable<PostViewModel> likeLine = UserLikes.GetLikesFor(Security.UserId).Skip(pageIndex * pageSize).Take(pageSize).ToArray();

            foreach (PostViewModel item in likeLine) {
                postIds.Add(item.PostId);
            }


            lastPageIndexLikes = pageIndex;
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

        public ActionResult Followers(int pageNo)
        {
            int pageSize = 10;

            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<User> followers = Users.GetFollowers(CurrentUser.Id).Skip((pageNo - 1) * pageSize).Take(pageSize);

            return View("Followers", new FollowViewModel() {
                FollowData = followers,
                FollowingCount = Users.GetFollowers(CurrentUser.Id).Count(),
                NextPage = pageNo + 1,
                PrevPage = pageNo - 1,
                PageSize = pageSize
            });
        }

        public ActionResult Following(int pageNo)
        {
            int pageSize = 10;

            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<User> following = Users.GetFollowing(CurrentUser.Id).Skip((pageNo - 1) * pageSize).Take(pageSize);

            return View("Following", new FollowViewModel() {
                FollowData = following,
                FollowingCount = Users.GetFollowing(CurrentUser.Id).Count(),
                NextPage = pageNo + 1,
                PrevPage = pageNo - 1,
                PageSize = pageSize
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
            if(ModelState.IsValid == true)
            {
                Post made = Posts.CreateTextPost(Security.UserId, model.TextPost);

                if(model.Hashtags != null)
                {


                    Hashtags.Create(made.Id, Security.UserId, model.Hashtags);
                }
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public ActionResult ImagePost() {
            return View("ImagePost", new CreateImagePostViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        [HttpGet]
        public JsonResult AutoComplete(string id)
        {
            List<string> results = new List<string>();

            results.AddRange(Users.SearchUsername(id));
            results.AddRange(Posts.GetTagTerms(id));

            results = results.OrderBy(q => q).Distinct(StringComparer.CurrentCultureIgnoreCase).Take(5).ToList();

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Settings()
        {
            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            User user = Users.GetBy(Security.UserId);
            SettingsViewModel settings = new SettingsViewModel()
            {
                BlogTitle = user.BlogTitle,
                Username = user.Username,
                Email = user.Email
            };

            return View("Settings", settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Settings(SettingsViewModel settings)
        {
            if (Security.Authenticate(CurrentUser.Username, settings.CurrentPassword) == false)
            {
                ModelState.AddModelError("Username", "The password was incorrect");
                return View("Settings", settings);
            }

            string avatarImageName = "projecktor_" + GetRandomString();
            HttpPostedFileBase avatarImage = settings.Avatar;

            string imageNameOriginal = "";
            string imageName128 = "";
            string filePathOriginal = "";
            string filePath128 = "";

            if(avatarImage != null)
            {
                imageNameOriginal = avatarImageName + Path.GetExtension(avatarImage.FileName);
                imageName128 = avatarImageName + "128" + Path.GetExtension(avatarImage.FileName);

                filePathOriginal = "~/Avatars/" + imageNameOriginal;
                filePath128 = "~/Avatars/" + imageName128;

                avatarImage.SaveAs(Server.MapPath(filePathOriginal));

                Bitmap Image128 = ResizeImage(Image.FromStream(avatarImage.InputStream, true, true), 128, 128);
                Image128.Save(Server.MapPath(filePath128));
            }

            Users.Settings(settings.Username, settings.BlogTitle, settings.NewPassword, settings.Email, filePathOriginal, Security.UserId);
            return View("Settings", settings);
        }

        [HttpGet]
        public ActionResult Activity()
        {
            if (Security.IsAuthenticated == false) {
                return RedirectToAction("Index", "Home");
            }

            List<ActivityViewModel> Activity = Users.Activity(CurrentUser.Id).Take(15).ToList();
            return View("Activity", Activity);
        }

        [HttpGet]
        public JsonResult GetActivityCheck(int pageIndex, int pageSize)
        {
            if (pageIndex * pageSize > Users.Activity(Security.UserId).Count()) {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetActivity(int pageIndex, int pageSize)
        {
            if (pageIndex * pageSize > Users.Activity(Security.UserId).Count()) {
                return Json(null);
            }

            List<int> activityIds = new List<int>();
            IEnumerable<ActivityViewModel> activtiyLine = Users.Activity(CurrentUser.Id).Skip(pageIndex * pageSize).Take(pageSize).ToArray();

            foreach (ActivityViewModel item in activtiyLine) {
                activityIds.Add(item.ActId);
            }

            return Json(activityIds.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowActivity(List<int> data)
        {
            List<ActivityViewModel> activities = new List<ActivityViewModel>();

            foreach (var number in data) {
                activities.Add(Users.Activity(CurrentUser.Id).Where(p => p.ActId == number).FirstOrDefault());
            }

            return PartialView("_MultiActivities", activities);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        public static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
        }

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