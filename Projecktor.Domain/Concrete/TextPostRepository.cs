using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Projecktor.Domain.Concrete
{
    public class TextPostRepository : EfRepository<TextPost>, ITextPostRepository
    {
        public TextPostRepository(DbContext context, bool sharedContext) : base(context, sharedContext) { }

        public void AddFor(TextPost textPost, User user) {
            user.TextPosts.Add(textPost);

            if(ShareContext == false) {
                Context.SaveChanges();
            }
        }

        public TextPost GetBy(int id) {
            return Find(t => t.Id == id);
        }

        public IEnumerable<TextPost> GetFor(User user) {
            return user.TextPosts.OrderByDescending(t => t.DateCreated);
        }
    }
}
