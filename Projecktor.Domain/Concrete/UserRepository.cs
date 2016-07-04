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

        public User GetBy(string username) {
            return Find(u => u.Username == username);
        }

        public User GetBy(int id) {
            return Find(u => u.Id == id);
        }
    }
}
