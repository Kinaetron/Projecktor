using Projecktor.Domain.Abstract;
using Projecktor.Domain.Concrete;
using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;
using Projecktor.WebUI.Infrastructure.Concrete;
using System.Web.Mvc;

namespace Projecktor.WebUI.Controllers
{
    public class ProjecktorControllerBase : Controller
    {
        public IContext DataContext;
        public IPostService Posts { get; private set; }
        public ILikeService UserLikes { get; private set; }
        public IHashtagService Hashtags { get; private set; }
        public User CurrentUser { get; set; }
        public IUserService Users { get; private set; }
        public ISecurityService Security { get; private set; }

        public ProjecktorControllerBase()
        {
            DataContext = new Context();
            Users = new UserService(DataContext);
            Posts = new PostService(DataContext);
            Hashtags = new HashtagService(DataContext);
            UserLikes = new LikeService(DataContext);
            Security = new SecurityService(Users);
            CurrentUser = Security.GetCurrentUser();
        }

        protected override void Dispose(bool disposing)
        {
            if(DataContext != null) {
                DataContext.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}