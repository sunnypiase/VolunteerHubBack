﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PostConnection> PostConnections { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>().HasData(
                new Tag { TagId = 1, Name = "Житло" },
                new Tag { TagId = 2, Name = "Медицина" });


            modelBuilder.Entity<PostConnection>()
            .HasOne(p => p.VolunteerPost)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<PostConnection>()
            .HasOne(p => p.NeedfulPost)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
