using Projecktor.Domain.Entites;

namespace Projecktor.Domain.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        User GetBy(int id);
        User GetBy(string username);
    }
}
