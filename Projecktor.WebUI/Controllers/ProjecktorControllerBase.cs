using Projecktor.Domain.Concrete;
using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;
using Projecktor.WebUI.Infrastructure.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projecktor.WebUI.Controllers
{
    public class ProjecktorControllerBase : Controller
    {
        public User CurrentUser { get; set; }
        public IUserService Users { get; private set; }
        public ISecurityService Security { get; private set; }

        public ProjecktorControllerBase()
        {
            Users = new UserService(new Context());
            Security = new SecurityService(Users);
            CurrentUser = Security.GetCurrentUser();
        }
    }
}