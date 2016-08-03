using Projecktor.Domain.Entites;

namespace Projecktor.Domain.Abstract
{
    public interface IPostRepository : IRepository<Post>
    {
        Post GetBy(int id);
    }
}
