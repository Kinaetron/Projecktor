using Projecktor.Domain.Entites;

namespace Projecktor.Domain.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        User GetBy(int id, bool includeTextPosts = false,
                   bool includeFollowers = false, bool includeFollowing = false);

        User GetBy(string username, bool includeTextPosts = false,
                   bool includeFollowers = false, bool includeFollowing = false);
    }
}
