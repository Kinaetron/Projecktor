using Projecktor.Domain.Entites;
using System.Data.Entity;

namespace Projecktor.Domain.Concrete
{
    public class ProjecktorDatabase : DbContext
    {
        public ProjecktorDatabase() : base("ProjecktorConnection") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Hashtag> HashTags { get; set; }
        public DbSet<Follow> Follow { get; set; }
        public DbSet<PasswordReset> PasswordReset { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.Follow);
            modelBuilder.Entity<User>().HasMany(u => u.Hashtags);
            modelBuilder.Entity<User>().HasMany(u => u.Likes);
            modelBuilder.Entity<User>().HasMany(u => u.Posts);
            modelBuilder.Entity<User>().HasMany(u => u.Texts);
            modelBuilder.Entity<User>().HasMany(u => u.PasswordReset);
        }
    }
}
