using Projecktor.Domain.Abstract;
using System.Data.Entity;


namespace Projecktor.Domain.Concrete
{
    public class Context : IContext
    {
        private readonly DbContext db;


        public Context(DbContext context = null, IUserRepository users = null, 
                       ITextPostRepository textPosts = null, ILikeRepository likes = null)
        {
            db = context ?? new ProjecktorDatabase();
            Users = users ?? new UserRepository(db, true);
            TextPosts = textPosts ?? new TextPostRepository(db, true);
            Likes = likes ?? new LikeRepository(db, true);
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

        public ILikeRepository Likes
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
