using System;
using System.Web.Helpers;
using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Projecktor.WebUI.Infrastructure.Concrete
{
    public class UserService : IUserService
    {
        private readonly IContext context;
        private readonly IUserRepository users;

        public UserService(IContext context)
        {
            this.context = context;
            users = context.Users;
        }

        public IEnumerable<User> AllUsers()
        {
            return users.AllUsers().ToArray();
        }

        public void Follow(string username, User follower)
        {
            users.CreateFollower(username, follower);
            context.SaveChanges();
        }

        public void Unfollow(string username, User follower)
        {
            users.DeleteFollower(username, follower);
            context.SaveChanges();
        }

        public User GetBy(int id) {
            return users.GetBy(id);
        }

        public User GetBy(string username) {
            return users.GetBy(username);
        }
        public User GetByEmail(string email) {
            return users.GetByEmail(email);
        }

        public User GetAllFor(int id)
        {
            return users.GetBy(id,
                               includeTextPosts: true,
                               includeFollowers: true,
                               includeFollowing: true);
        }

        public User GetAllFor(string username)
        {
            return users.GetBy(username,
                               includeTextPosts: true,
                               includeFollowers: true,
                               includeFollowing: true);
        }

        public IEnumerable<User> SearchFor(string username)
        {
            return users.FindAll(u => u.Username.Contains(username));
        }

        public IEnumerable<string> SearchUsername(string username)
        {
            List<string> names = new List<string>();
            List<User> userList = users.FindAll(u => u.Username.Contains(username)).ToList();

            foreach (User user in userList) {
                names.Add(user.Username);
            }

            return names;
        }

        public User Settings(string username, string password, string email, int userId)
        {
            User user = users.Find(u => u.Id == userId);

            if(username != null) {
               user.Username = username;
            }
            if(password != null) {
                user.Password = Crypto.HashPassword(password);
            }
            if(email != null) {
                user.Email = email;
            }
            context.SaveChanges();

            return user;
        }

        public User Create(string username, string password, string email, DateTime? created = null)
        {
            User user = new User()
            {
                Username = username,
                Password = Crypto.HashPassword(password),
                Email = email,
                DateCreated = created.HasValue ? created.Value : DateTime.Now,
            };

            users.Create(user);
            context.SaveChanges();

            return user;
        }
    }
}