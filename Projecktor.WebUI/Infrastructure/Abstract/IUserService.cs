using System;
using Projecktor.Domain.Entites;
using System.Collections.Generic;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface IUserService
    {
        IEnumerable<User> AllUsers();
        void Follow(string username, User Follower);
        void Unfollow(string username, User Unfollower);
        User GetBy(int id);
        User GetBy(string username);
        User GetByEmail(string email);
        User GetAllFor(int id);
        User GetAllFor(string username);
        IEnumerable<User> SearchFor(string username);
        User Settings(string username, string password, string email, int userId);
        User Create(string username, string password, string email, DateTime? created = null);
    }
}
