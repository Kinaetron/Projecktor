using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;
using Projecktor.WebUI.Models;
using System;
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
            User user = null;

            if(username.Contains("@") == true) {
                user = users.GetByEmail(username);
            }
            else {
                user = users.GetBy(username);
            }

            if (user == null) {
                return false;
            }

            return Crypto.VerifyHashedPassword(user.Password, password);
        }

        public User CreateUser(RegisterViewModel signupModel, bool login = true)
        {
            User user = users.Create(signupModel.Username, signupModel.Password, signupModel.Email);

            if (login == true) {
                Login(user);
            }

            return user;
        }

        public User GetCurrentUser() {
            return users.GetBy(UserId);
        }

        public bool DoesUserExist(string username)
        {
            bool result = users.GetBy(username) != null;

            if(result == false) {
                result = users.GetByEmail(username) != null;
            }
            return result;
        }

        public void Login(string username)
        {
            User user = null;

            if (username.Contains("@") == true) {
                user = users.GetByEmail(username);
            }
            else {
                user = users.GetBy(username);
            }

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