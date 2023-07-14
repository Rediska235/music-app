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

    public async Task<Artist> GetArtistByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await _db.Artists.FirstOrDefaultAsync(a => a.Username == username, cancellationToken);
    }

    public async Task CreateArtistAsync(Artist artist, CancellationToken cancellationToken)
    {
        await _db.AddAsync(artist, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}
