using Microsoft.EntityFrameworkCore;
using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Song> Songs { get; set; }
    public DbSet<Artist> Artists { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
