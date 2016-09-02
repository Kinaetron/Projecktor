using System;
using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using System.Collections.Generic;
using System.Data.Entity;

namespace Projecktor.Domain.Concrete
{
    public class LikeRepository : EfRepository<Like>, ILikeRepository {
        public LikeRepository(DbContext context, bool sharedContext) : base(context, sharedContext) { }
    }
}
