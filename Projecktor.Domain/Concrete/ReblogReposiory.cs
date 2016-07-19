using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using System.Data.Entity;

namespace Projecktor.Domain.Concrete
{
    public class ReblogReposiory : EfRepository<Reblog>, IReblogRepository 
    {
        public ReblogReposiory(DbContext context, bool sharedContext) : base(context, sharedContext) { }
    }
}
