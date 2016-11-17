using Projecktor.Domain.Entites;
using System.Linq;

namespace Projecktor.Domain.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        IQueryable<User> AllUsers();

        User GetBy(int id, bool includeTextPosts = false);
        User GetBy(string username, bool includeTextPosts = false);
        User GetByEmail(string email, bool includeTextPosts = false);
    }
}
