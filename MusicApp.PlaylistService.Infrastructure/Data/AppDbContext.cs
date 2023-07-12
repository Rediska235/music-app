using Microsoft.EntityFrameworkCore;
using MusicApp.PlaylistService.Domain.Entities;

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
        modelBuilder.Entity<Song>()
        .HasMany(f => f.Playlists)
        .WithMany(g => g.Songs)
        .UsingEntity<PlaylistSong>(
            j => j.HasOne<Playlist>().WithMany().OnDelete(DeleteBehavior.Cascade),
            j => j.HasOne<Song>().WithMany().OnDelete(DeleteBehavior.Cascade));
    }
}