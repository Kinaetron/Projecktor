using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.SessionState;

namespace Projecktor.WebUI.Infrastructure.Concrete
{
    public class SecurityService : ISecurityService
    {
        private readonly IUserService users;
        private readonly HttpSessionState session;

        public bool IsAuthenticated
        {
            get { return UserId > 0; }
        }

        public int UserId
        {
            get { return Convert.ToInt32(session["UserId"]); }
            set { session["UserId"] = value; }
        }

        public SecurityService(IUserService users, HttpSessionState session = null)
        {
            this.users = users;
            this.session = session ?? HttpContext.Current.Session;
        }

        public bool Authenticate(string username, string password)
        {
            var user = users.GetBy(username);

            if (user == null) {
                return false;
            }

            return Crypto.VerifyHashedPassword(user.Password, password);
        }

        public User CreateUser(string username, string password, bool login = true)
        {
            var user = users.Create(username, password);

            if (login == true) {
                Login(user);
            }

            return user;
        }

        public User GetCurrentUser() {
            return users.GetBy(UserId);
        }

        public bool DoesUserExist(string username) {
            return users.GetBy(username) != null;
        }

        public void Login(string username)
        {
            var user = users.GetBy(username);
            Login(user);
        }

        public void Login(User user) {
            session["UserId"] = user.Id;
        }

        public void Logout() {
            session.Abandon();
        }
    }
}