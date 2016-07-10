using Projecktor.Domain.Entites;
using System.Linq;

namespace Projecktor.Domain.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        IQueryable<User> AllUsers();

        void CreateFollower(string username, User follower);
        void DeleteFollower(string username, User follower);

        User GetBy(int id, bool includeTextPosts = false,
                   bool includeFollowers = false, bool includeFollowing = false);

        User GetBy(string username, bool includeTextPosts = false,
                   bool includeFollowers = false, bool includeFollowing = false);
    }
}
