using Microsoft.EntityFrameworkCore;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Infrastructure.Extensions;
using System.Diagnostics;

namespace MusicApp.PlaylistService.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<User> Users{ get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Seed();
    }
}