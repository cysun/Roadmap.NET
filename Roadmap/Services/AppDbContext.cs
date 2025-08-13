using Microsoft.EntityFrameworkCore;
using Roadmap.Models;

namespace Roadmap.Services;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.CampusId).IsUnique();
        modelBuilder.Entity<User>().Property(u => u.IsVerified).HasDefaultValue(false);
    }
}