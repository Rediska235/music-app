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

    public async Task<IEnumerable<Song>> GetAllSongsAsync()
    {
        return await _db.Songs.Include(s => s.Artist).ToListAsync();
    }

    public async Task<Song> GetSongByIdAsync(Guid id)
    {
        return await _db.Songs.Include(s => s.Artist).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task CreateSongAsync(Song song)
    {
        await _db.AddAsync(song);
    }

    public void UpdateSong(Song song)
    {
        _db.Update(song);
    }
    
    public void DeleteSong(Song song)
    {
        _db.Remove(song);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
