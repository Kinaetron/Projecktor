using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using System.Data.Entity;
using System.Linq;

namespace Projecktor.Domain.Concrete
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context, bool sharedContext) : base(context, sharedContext) { }

        public IQueryable<User> AllUsers() {
            return All();
        }

        public User GetBy(string username, bool includeTextPosts = false)
        {
            IQueryable<User> query = BuildUserQuery(includeTextPosts);
            return query.SingleOrDefault(u => u.Username == username);
        }

        public User GetBy(int id, bool includeTextPosts = false)
        {
            IQueryable<User> query = BuildUserQuery(includeTextPosts);
            return query.SingleOrDefault(u => u.Id == id);
        }

        public User GetByEmail(string email, bool includeTextPosts = false)
        {
            IQueryable<User> query = BuildUserQuery(includeTextPosts);
            return query.SingleOrDefault(u => u.Email == email);
        }

        private IQueryable<User> BuildUserQuery(bool includeTextPosts)
        {
            var query = DbSet.AsQueryable();

            if (includeTextPosts == true) {
                query = DbSet.Include(u => u.Posts);
            }

            return query;
        }
    }
}
