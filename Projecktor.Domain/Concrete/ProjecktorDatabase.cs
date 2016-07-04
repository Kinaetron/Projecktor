using Projecktor.Domain.Entites;
using System.Data.Entity;

namespace Projecktor.Domain.Concrete
{
    public class ProjecktorDatabase : DbContext
    {
        public ProjecktorDatabase() : base("ProjecktorConnection") { }

        public DbSet<User> Users { get; set; }
        public DbSet<TextPost> TextPosts { get; set; }

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

            modelBuilder.Entity<User>().HasMany(u => u.TextPosts);
            base.OnModelCreating(modelBuilder);
        }
    }
}
