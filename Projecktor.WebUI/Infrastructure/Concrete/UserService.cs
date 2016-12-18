using System;
using System.Web.Helpers;
using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using Projecktor.WebUI.Models;
using Projecktor.WebUI.Infrastructure.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Projecktor.WebUI.Infrastructure.Concrete
{
    public class UserService : IUserService
    {
        private readonly IContext context;
        private readonly IUserRepository users;
        private readonly IFollowRepository follows;
        private readonly ILikeRepository likes;
        private readonly IPostRepository posts;

        public UserService(IContext context)
        {
            this.context = context;
            users = context.Users;
            follows = context.Follow;
            likes = context.Likes;
            posts = context.Posts;
        }

        public IEnumerable<User> AllUsers()
        {
            return users.AllUsers().ToArray();
        }

        public void Follow(string username, User follower)
        {
            User following = GetBy(username);

            var follow = new Follow()
            {
                FollowingId = following.Id,
                FollowerId = follower.Id,
                DateDone = DateTime.Now
            };

            follows.Create(follow);    
            context.SaveChanges();
        }

        public void Unfollow(string username, User follower)
        {
            User following = GetBy(username);
            follows.Delete(follows.Find(f => f.FollowingId == following.Id && f.FollowerId == follower.Id));
            context.SaveChanges();
        }

        public void PasswordReset(string password, int userId)
        {
            User user = users.Find(u => u.Id == userId);
            user.Password = Crypto.HashPassword(password);
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

        public User GetAllFor(int id) {
            return users.GetBy(id, includeTextPosts: true);
        }

        public User GetAllFor(string username) {
            return users.GetBy(username, includeTextPosts: true);
        }

        public IEnumerable<User> SearchFor(string username) {
            return users.FindAll(u => u.Username.Contains(username));
        }

        public IEnumerable<string> SearchUsername(string username)
        {
            List<string> names = new List<string>();
            List<User> userList = users.FindAll(u => u.Username.StartsWith(username)).ToList();

            foreach (User user in userList) {
                names.Add(user.Username);
            }

            return names;
        }

        public IEnumerable<User> GetFollowers(int userId)
        {
            List<Follow> followers = follows.FindAll(f => f.FollowingId == userId).ToList();
            List<User> followerDetails = new List<User>();

            foreach (var follower in followers) {
                followerDetails.Add(users.Find(u => u.Id == follower.FollowerId));
            }

            return followerDetails;
        }

        public IEnumerable<User> GetFollowing(int userId)
        {
            List<Follow> followings = follows.FindAll(f => f.FollowerId == userId).ToList();
            List<User> followingDetails = new List<User>();

            foreach (var following in followings) {
                followingDetails.Add(users.Find(u => u.Id == following.FollowingId));
            }

            return followingDetails;
        }

        public bool isFollowing(int userId, int followerId) {
            return follows.FindAll(f => f.FollowerId == userId && f.FollowingId == followerId).Any();
        }

        public User Settings(string username, string blogTitle, string password, string email, string avatar, int userId)
        {
            User user = users.Find(u => u.Id == userId);

            if(username != null) {
               user.Username = username;
            }
            if(blogTitle != null) {
                user.BlogTitle = blogTitle;
            }
            if(password != null) {
                user.Password = Crypto.HashPassword(password);
            }
            if(email != null) {
                user.Email = email;
            }
            if(avatar != "") {
                user.Avatar = avatar;
            }
            context.SaveChanges();

            return user;
        }

        public User Create(string username, string password, string email, DateTime? created = null)
        {
            User user = new User()
            {
                Username = username,
                BlogTitle = username,
                Password = Crypto.HashPassword(password),
                Email = email,
                DateCreated = created.HasValue ? created.Value : DateTime.Now,
                Avatar = "~/Avatars/default.png",
            };

            users.Create(user);
            context.SaveChanges();

            return user;
        }

        public IEnumerable<ActivityViewModel> Activity(int userId)
        {
            List<ActivityViewModel> activityList = new List<ActivityViewModel>();

            List<Like> LikesAct = likes.FindAll(l => l.SourceId == userId).ToList();
            List<Post> postsAct = posts.FindAll(p => p.ReblogId == userId).ToList();
            List<Follow> followAct = follows.FindAll(f => f.FollowingId == userId).ToList();

            foreach (var like in LikesAct)
            {
                ActivityViewModel act = new ActivityViewModel()
                {
                    Date = like.DateCreated,
                    PostId = like.PostId,
                    Action = ActionEnum.Like,
                    From = GetBy(like.UserId),
                };
                activityList.Add(act);
            }

            foreach (var reblog in postsAct)
            {
                ActivityViewModel act = new ActivityViewModel()
                {
                    Date = reblog.DateCreated,
                    PostId = reblog.Id,
                    SourceId = reblog.ReblogId,
                    Action = ActionEnum.Reblog,
                    From = GetBy(reblog.AuthorId),
                };
                activityList.Add(act);
            }

            foreach (var follow in followAct)
            {
                ActivityViewModel act = new ActivityViewModel()
                {
                    Date = follow.DateDone,
                    Action = ActionEnum.Followed,
                    From = GetBy(follow.FollowerId),
                };
                activityList.Add(act);
            }

            return activityList.OrderByDescending(d => d.Date);
        }
    }
}