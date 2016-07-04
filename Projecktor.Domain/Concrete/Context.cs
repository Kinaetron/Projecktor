using Projecktor.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecktor.Domain.Concrete
{
    public class Context : IContext
    {
        private readonly DbContext db;


        public Context(DbContext context = null, IUserRepository users = null, 
                       ITextPostRepository textPosts = null)
        {
            db = context ?? new ProjecktorDatabase();
            Users = users ?? new UserRepository(db, true);
            TextPosts = textPosts ?? new TextPostRepository(db, true);
        }

        public ITextPostRepository TextPosts
        {
            get;
            private set;
        }

        public IUserRepository Users
        {
            get;
            private set;
        }

        public int SaveChanges() {
            return db.SaveChanges();
        }

        public void Dispose()
        {
            if (db != null)
            {
                try {
                    db.Dispose();
                }
                catch{ }
            }
        }
    }
}
