using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using System.Data.Entity;

namespace Projecktor.Domain.Concrete
{
    public class FollowRepository : EfRepository<Follow>, IFollowRepository {
        public FollowRepository(DbContext context, bool sharedContext) : base(context, sharedContext) { }
    }
}
