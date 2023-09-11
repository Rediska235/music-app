using Microsoft.EntityFrameworkCore;
using MusicApp.Identity.Domain.Entities;
using MusicApp.Identity.Infrastructure.Extensions;

namespace MusicApp.Identity.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Seed();
    }
}
