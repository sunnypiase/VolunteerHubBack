using Microsoft.EntityFrameworkCore;

namespace Core.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            /*Database.EnsureDeleted();
            Database.EnsureCreated();*/
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>().HasData(
                new Tag { TagId = 1, Name = "Житло" },
                new Tag { TagId = 2, Name = "Медицина" });
        }
    }
}
