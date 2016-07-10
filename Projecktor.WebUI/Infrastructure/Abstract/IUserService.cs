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
        User GetAllFor(int id);
        User GetAllFor(string username);
        User Create(string username, string password, DateTime? created = null);
    }
}
