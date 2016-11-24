using System;
using Projecktor.Domain.Entites;
using System.Collections.Generic;
using Projecktor.WebUI.Models;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface IUserService
    {
        IEnumerable<User> AllUsers();
        void Follow(string username, User Follower);
        void Unfollow(string username, User Unfollower);
        void PasswordReset(string password, int userId);
        User GetBy(int id);
        User GetBy(string username);
        User GetByEmail(string email);
        User GetAllFor(int id);
        User GetAllFor(string username);
        IEnumerable<User> GetFollowers(int userId);
        IEnumerable<User> GetFollowing(int userId);
        bool isFollowing(int userId, int followerId);
        IEnumerable<User> SearchFor(string username);
        IEnumerable<string> SearchUsername(string username);
        User Settings(string username, string password, string email, int userId);
        User Create(string username, string password, string email, DateTime? created = null);
        IEnumerable<ActivityViewModel> Activity(int userId);
    }
}
