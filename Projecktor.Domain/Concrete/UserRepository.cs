using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecktor.Domain.Concrete
{
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context, bool sharedContext) : base(context, sharedContext) { }

        public IQueryable<User> AllUsers() {
            return All();
        }

        public void CreateFollower(string username, User follower)
        {
            var user = GetBy(username);
            DbSet.Attach(follower);

            user.Followers.Add(follower);

            if (ShareContext == false) {
                Context.SaveChanges();
            }
        }

        public void DeleteFollower(string username, User follower)
        {
            var user = GetBy(username);
            DbSet.Attach(follower);

            user.Followers.Remove(follower);

            if (ShareContext == false) {
                Context.SaveChanges();
            }
        }

        public User GetBy(string username, bool includeTextPosts = false,
                          bool includeFollowers = false, bool includeFollowing = false)
        {
            IQueryable<User> query = BuildUserQuery(includeTextPosts, includeFollowers, includeFollowing);
            return query.SingleOrDefault(u => u.Username == username);
        }

        public User GetBy(int id, bool includeTextPosts = false,
                          bool includeFollowers = false, bool includeFollowing = false)
        {
            IQueryable<User> query = BuildUserQuery(includeTextPosts, includeFollowers, includeFollowing);
            return query.SingleOrDefault(u => u.Id == id);
        }

        private IQueryable<User> BuildUserQuery(bool includeTextPosts, bool includeFollowers, bool includeFollowing)
        {
            var query = DbSet.AsQueryable();

            if (includeTextPosts == true) {
                query = DbSet.Include(u => u.TextPosts);
            }

            if (includeFollowers == true) {
                query = DbSet.Include(u => u.Followers);
            }

            if (includeFollowing == true) {
                query = DbSet.Include(u => u.Following);
            }

            return query;
        }
    }
}
