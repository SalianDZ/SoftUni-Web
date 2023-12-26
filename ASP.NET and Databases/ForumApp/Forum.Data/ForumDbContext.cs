using ForumApp.Data.Configuration;
using ForumApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Data
{
    public class ForumDbContext : DbContext
    {
        public ForumDbContext(DbContextOptions<ForumDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Post> Posts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PostEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
