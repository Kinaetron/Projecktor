using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using System.Data.Entity;

namespace Projecktor.Domain.Concrete
{
    public class PostRepository : EfRepository<Post>, IPostRepository
    {
        public PostRepository(DbContext context, bool sharedContext) : base(context, sharedContext) { }

        public Post GetBy(int id) {
            return Find(t => t.Id == id);
        }
    }
}
