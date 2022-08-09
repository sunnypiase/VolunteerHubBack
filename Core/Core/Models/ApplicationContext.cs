using Microsoft.EntityFrameworkCore;

namespace Core.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Житло" },
                new Category { CategoryId = 2, Name = "Медицина" });
            modelBuilder.Entity<Category>().HasKey(category => category.CategoryId);
        }
    }
}
