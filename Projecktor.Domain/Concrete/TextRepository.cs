using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using System.Data.Entity;

namespace Projecktor.Domain.Concrete
{
    public class TextRepository : EfRepository<Text>, ITextRepository {
        public TextRepository(DbContext context, bool sharedContext) : base(context, sharedContext) { }
    }
}
