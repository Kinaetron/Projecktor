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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Followers)
                .WithMany(u => u.Following)
                .Map(map =>
                {
                    map.MapLeftKey("FollowingId");
                    map.MapRightKey("FollowerId");
                    map.ToTable("Follow");
                });

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasMany(u => u.Hashtags);
            modelBuilder.Entity<User>().HasMany(u => u.Likes);
            modelBuilder.Entity<User>().HasMany(u => u.Posts);
            modelBuilder.Entity<User>().HasMany(u => u.Texts);
        }
    }
}
