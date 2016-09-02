using System.Data.Entity;

using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;

namespace Projecktor.Domain.Concrete
{
    public class HashtagRepository : EfRepository<Hashtag>, IHashtagRepository {
        public HashtagRepository(DbContext context, bool sharedContext) : base(context, sharedContext) { }
    }
}
