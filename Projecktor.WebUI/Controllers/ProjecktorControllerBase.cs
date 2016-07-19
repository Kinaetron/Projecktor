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
        public ITextPostService TextPosts { get; private set; }
        public ILikeService UserLikes { get; private set; }
        public IReblogService UserReblogs { get; private set; }
        public User CurrentUser { get; set; }
        public IUserService Users { get; private set; }
        public ISecurityService Security { get; private set; }

        public ProjecktorControllerBase()
        {
            DataContext = new Context();
            Users = new UserService(DataContext);
            TextPosts = new TextPostService(DataContext);
            UserLikes = new LikeService(DataContext);
            UserReblogs = new ReblogService(DataContext);
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