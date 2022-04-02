using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace NewsWebsite.Models
{
    public partial class NewsDbContext : DbContext
    {
        public NewsDbContext()
            : base("name=NewsDbContext1")
        {
        }

        public virtual DbSet<WebInfo> infoes { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<StickyPost> StickyPosts { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasMany(e => e.StickyPosts)
                .WithRequired(e => e.Post)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Posts)
                .Map(m => m.ToTable("Tbl_PostTags").MapLeftKey("post_id").MapRightKey("tag_id"));
            
            modelBuilder.Entity<Post>()
                .HasMany(e => e.Series)
                .WithMany(e => e.Posts)
                .Map(m => m.ToTable("Tbl_SeriesPost").MapLeftKey("post_id").MapRightKey("series_id"));

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.password)
                .IsUnicode(false);
        }
    }
}
