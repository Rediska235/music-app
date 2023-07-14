using Microsoft.EntityFrameworkCore;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Infrastructure.Data;

namespace MusicApp.SongService.Infrastructure.Repositories;

public class SongRepository : ISongRepository
{
    private readonly AppDbContext _db;

    public SongRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Song>> GetSongsAsync(CancellationToken cancellationToken)
    {
        return await _db.Songs
            .Include(s => s.Artist)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Song> GetSongByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.Songs
            .Include(s => s.Artist)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task CreateSongAsync(Song song, CancellationToken cancellationToken)
    {
        await _db.AddAsync(song, cancellationToken);
    }

    public void UpdateSong(Song song)
    {
        _db.Update(song);
    }
    
    public void DeleteSong(Song song)
    {
        _db.Remove(song);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}
