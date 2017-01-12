using System;
using System.Web;
using System.Text;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

using Projecktor.WebUI.Models;
using Projecktor.Domain.Entites;

using SendGrid;
using SendGrid.Helpers.Mail;
using System.Diagnostics;

namespace Projecktor.WebUI.Controllers
{
    public class HomeController : ProjecktorControllerBase
    {
        public HomeController() : base() { }

        public ActionResult Index(string subdomain)
        {
            HttpCookie cookie = Request.Cookies["loggedIn"];

            if (Security.IsAuthenticated == false && subdomain == null)
            {
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddYears(1);
                    cookie.Domain = ".projecktor.com";
                    Response.Cookies.Add(cookie);

                    Security.Login(cookie.Value);
                    return RedirectToAction("Index", "Dashboard");
                }

                return View("Register", new RegisterViewModel());
            }


            if (Security.IsAuthenticated == true && subdomain == null) {
                return RedirectToAction("Index", "Dashboard");
            }

            if (Security.IsAuthenticated == false && subdomain != null) {
                if (cookie != null) {
                    Security.Login(cookie.Value);
                }
            }

            User user = Users.GetAllFor(subdomain);

            if (user == null) {
                return new HttpNotFoundResult();
            }


            ExternalViewModel posts = new ExternalViewModel()
            {
                SubdomainUser = user,
                Posts = Posts.GetPostsFor(user.Id).Take(10).ToList(),
            };

            return View("UserBlogPage", posts);
        }

        [HttpGet]
        public JsonResult GetUserPosts(string subdomain, int pageIndex, int pageSize)
        {
            User user = Users.GetAllFor(subdomain);

            if (pageIndex * pageSize > Posts.GetPostsFor(user.Id).Count()) {
                return Json(null);
            }

            List<int> postIds = new List<int>();

            var timeline = Posts.GetPostsFor(user.Id).Skip(pageIndex * pageSize).Take(pageSize).ToArray();

            foreach (PostViewModel item in timeline) {
                postIds.Add(item.PostId);
            }

            return Json(postIds.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserPostsCheck(string subdomain, int pageIndex, int pageSize)
        {
            User user = Users.GetAllFor(subdomain);

            if (pageIndex * pageSize > Posts.GetPostsFor(user.Id).Count()) {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Likes(string subdomain)
        {
            if (Security.IsAuthenticated == false && subdomain == null) {
                return View("Register", new RegisterViewModel());
            }

            User user = Users.GetAllFor(subdomain);
            IEnumerable<PostViewModel> likeLine = UserLikes.GetLikesFor(user.Id).Take(10).ToArray();
            return View("UserBlogPage", likeLine);
        }

        public ActionResult Search(string id, string subdomain)
        {
            if (id == null && subdomain == null) {
                return RedirectToAction("Index", "Dashboard");
            }
            else if(id == "") {
                return Redirect("http://" + subdomain + ".projecktor.com");
            }

            string searchTerm = Uri.UnescapeDataString(id);

            if(subdomain == null)
            {
                SearchModel model = new SearchModel()
                {
                    FoundPosts = Posts.GetTagged(searchTerm).Take(10),
                    FoundUsers = Users.SearchFor(searchTerm).Take(5)
                };

                return View("SearchPage", model);
            }
            else
            {
                SearchModel model = new SearchModel() {
                    SubdomainUser = Users.GetBy(subdomain),
                    FoundPosts = Posts.GetTaggedUser(searchTerm, subdomain).Take(10),
                };

                return View("SearchPageExternal", model);
            }
        }


        [HttpGet]
        public JsonResult GetUserLikes(string subdomain, int pageIndex, int pageSize)
        {
            User user = Users.GetAllFor(subdomain);

            if (pageIndex * pageSize > UserLikes.GetLikesFor(user.Id).Count()) {
                return Json(null);
            }

            List<int> postIds = new List<int>();
            IEnumerable<PostViewModel> likeLine = UserLikes.GetLikesFor(user.Id).Skip(pageIndex * pageSize).Take(pageSize).ToArray();

            foreach (PostViewModel item in likeLine) {
                postIds.Add(item.PostId);
            }

            return Json(postIds.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserLikesCheck(string subdomain, int pageIndex, int pageSize)
        {
            User user = Users.GetAllFor(subdomain);

            if (pageIndex * pageSize > UserLikes.GetLikesFor(user.Id).Count()) {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Reblog(int textId, int reblogId, int sourceId)
        {
            Post madeReblog = Posts.Reblog(CurrentUser.Id, textId, reblogId, sourceId);
            Hashtag[] tags = Hashtags.GetHashTagsFor(sourceId).ToArray();

            if (tags != null) {
                Hashtags.Create(madeReblog.Id, Security.UserId, tags);
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
        public JsonResult Like(int postId, int sourceId)
        {
            if (ModelState.IsValid == true) {
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

        [HttpPost]
        public JsonResult Follow(string username)
        {

            Users.Follow(username, Security.GetCurrentUser());
            return Json(new { msg = "Successful" });
        }

        [HttpPost]
        public JsonResult UnFollow(string username)
        {
            Users.Unfollow(username, Security.GetCurrentUser());
            return Json(new { msg = "Successful" });
        }

        [HttpPost]
        public JsonResult DeletePost(int postId)
        {
            Post post = Posts.Getby(postId);

            if (post.Image1 != null)
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
            UserLikes.Delete(postId);

            return Json(new { msg = "Successful" });
        }

        [HttpGet]
        public JsonResult Gallery(string id)
        {
            Post post = Posts.Getby(int.Parse(id));
            List<string> images = new List<string>();

            string[] image;

            if(Debugger.IsAttached == true)
            {
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

            }
            else
            {
                if (post.Image1 != null)
                {
                    image = post.Image1.Remove(0, 1).Split('.');
                    images.Add(Url.Content(ProjecktorCDN + image[0] + "_720." + image[1]));
                }
                if (post.Image2 != null)
                {
                    image = post.Image2.Remove(0, 1).Split('.');
                    images.Add(Url.Content(ProjecktorCDN + image[0] + "_720." + image[1]));
                }
                if (post.Image3 != null)
                {
                    image = post.Image3.Remove(0, 1).Split('.');
                    images.Add(Url.Content(ProjecktorCDN + image[0] + "_720." + image[1]));
                }
                if (post.Image4 != null)
                {
                    image = post.Image4.Remove(0, 1).Split('.');
                    images.Add(Url.Content(ProjecktorCDN + image[0] + "_720." + image[1]));
                }
                if (post.Image5 != null)
                {
                    image = post.Image5.Remove(0, 1).Split('.');
                    images.Add(Url.Content(ProjecktorCDN + image[0] + "_720." + image[1]));
                }
                if (post.Image6 != null)
                {
                    image = post.Image6.Remove(0, 1).Split('.');
                    images.Add(Url.Content(ProjecktorCDN + image[0] + "_720." + image[1]));
                }
            }

            return Json(images, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowUserPost(List<int> data)
        {
            List<PostViewModel> models = new List<PostViewModel>();

            foreach (var number in data) {
                models.Add(Posts.GetPost(number));
            }

            return PartialView("_MultiPostsExternal", models);
        }

        public ActionResult Tagged(string subdomain, string id)
        {
            IEnumerable<PostViewModel> taggedPosts;

            string tagTerm = Uri.UnescapeDataString(id);

            if (subdomain == null)
            {
                taggedPosts = Posts.GetTagged(tagTerm).Take(10);
                return View("TaggedPage", taggedPosts);
            }
            else
            {
                taggedPosts = Posts.GetTaggedUser(tagTerm, subdomain);

                ExternalViewModel posts = new ExternalViewModel()
                {
                    SubdomainUser = Users.GetAllFor(subdomain),
                    Posts = Posts.GetTaggedUser(tagTerm, subdomain).Take(10),
                };

                return View("TaggedPageExternal", posts);
            }
        }

        [HttpGet]
        public JsonResult GetTaggedCheck(string subdomain, string term, int pageIndex, int pageSize)
        {
            string tagTerm = String.Concat(term.Where(ch => Char.IsLetterOrDigit(ch)));

            if (subdomain == null)
            {
                if (pageIndex * pageSize > Posts.GetTagged(tagTerm).Count()) {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (pageIndex * pageIndex > Posts.GetTaggedUser(tagTerm, subdomain).Count()) {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public JsonResult GetTagged(string subdomain, string term, int pageIndex, int pageSize)
        {
            IEnumerable<PostViewModel> taggedPosts;

            string tagTerm = String.Concat(term.Where(ch => Char.IsLetterOrDigit(ch)));

            if (subdomain == null)
            {
                List<int> postIds = new List<int>();

                taggedPosts = Posts.GetTagged(tagTerm).Skip(pageIndex * pageSize).Take(pageSize);

                foreach (PostViewModel item in taggedPosts) {
                    postIds.Add(item.PostId);
                }

                return Json(postIds.ToArray(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<int> postIds = new List<int>();

                taggedPosts = Posts.GetTaggedUser(tagTerm, subdomain).Skip(pageIndex * pageSize).Take(pageSize);

                foreach (PostViewModel item in taggedPosts) {
                    postIds.Add(item.PostId);
                }

                return Json(postIds.ToArray(), JsonRequestBehavior.AllowGet);
            }
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

            PostViewModel post = Posts.GetPost(int.Parse(id));

            ViewModelPostEx postEx = new ViewModelPostEx()
            {
                SubdomainUser = user,
                Post = post
            };

            if (user.Id != post.Author.Id) {
                return new HttpNotFoundResult();
            }
            return View("Post", postEx);
        }

        public ActionResult Notes(string id)
        {
            IEnumerable<Note> notes = Posts.Notes(int.Parse(id));
            return View("Notes", notes);
        }

        public ActionResult Image(string path)
        {
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
            HttpContext.Response.Cache.SetMaxAge(TimeSpan.FromDays(365));
            HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(365));

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
        public ActionResult ForgotPassword() {
            return View("ForgotPassword", new ForgotPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model) {
        
            if (Security.DoesUserExist(model.Email) == false) {
                ModelState.AddModelError("Email", "This email address doesn't exist on our system");
                return View("ForgotPassword", new ForgotPasswordViewModel());
            }

            User user = Users.GetByEmail(model.Email);
            SendEmail(user, PasswordResets.Create(user.Id)).Wait();
            return RedirectToAction("Index", "Home");
        }
         
        [HttpGet]
        public ActionResult PasswordReset(int userId = 0, int passwordId = 0)
        {
            if (passwordId == 0 && userId == 0 || Security.DoesUserExist(userId) == false) {
                return RedirectToAction("ForgotPassword", "Home");
            }

            if(Security.DoesUserExist(userId) == true && PasswordResets.Exist(passwordId) == false)
            {
                PasswordRestViewModel model = new PasswordRestViewModel() {
                    ExpiredLink = true
                };
                return View("PasswordReset", model);
            }

            PasswordResets.Delete(passwordId);
            return View("PasswordReset", new PasswordRestViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordReset(PasswordRestViewModel model)
        {
            if(model.Password.Equals(model.ConfirmPassword, StringComparison.CurrentCulture) == false) {
                ModelState.AddModelError("Password", "These passwords don't match each other");
                return View("PasswordReset", new PasswordRestViewModel());
            }

            Users.PasswordReset(model.Password, model.userId);
            Security.Login(Users.GetBy(model.userId));

            return RedirectToAction("Index", "Dashboard");
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


        public static async Task SendEmail(User user, int passwordId)
        {
            string apiKey = Environment.GetEnvironmentVariable("ProjecktorMailKey");
            dynamic sg = new SendGridAPIClient(apiKey);

            StringBuilder body = new StringBuilder();
            body.Append("Forgot your password? Reset it below:");
            body.Append(Environment.NewLine);
            body.Append(Environment.NewLine);
            body.Append("projecktor.com/passwordreset/" + passwordId + "/" + user.Id);

            Email from = new Email("no-reply@projecktor.com", "Projecktor");
            string subject = "Projecktor Password";
            Email to = new Email(user.Email);
            Content content = new Content("text/plain", body.ToString());
            Mail mail = new Mail(from, subject, to, content);

            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
        }
    }
}