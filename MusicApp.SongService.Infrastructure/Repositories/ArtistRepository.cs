using Microsoft.EntityFrameworkCore;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Infrastructure.Data;

namespace MusicApp.SongService.Infrastructure.Repositories;

public class ArtistRepository : IArtistRepository
{
    private readonly AppDbContext _db;

    public ArtistRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Artist> GetArtistbyUsername(string username)
    {
        return await _db.Artists.FirstOrDefaultAsync(a => a.Username == username);
    }

    public void CreateArtist(Artist artist)
    {
        _db.Add(artist);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
