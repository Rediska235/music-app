using Microsoft.EntityFrameworkCore;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Infrastructure.Data;

namespace MusicApp.PlaylistService.Infrastructure.Repositories;

public class PlaylistRepository : IPlaylistRepository
{
    private readonly AppDbContext _db;

    public PlaylistRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Playlist>> GetPublicPlaylistsAsync()
    {
        return await _db.Playlists.Include(p => p.Creator).Where(p => !p.IsPrivate).ToListAsync();
    }
    public async Task<IEnumerable<Playlist>> GetMyPrivatePlaylistsAsync(string username)
    {
        return await _db.Playlists.Include(p => p.Creator).Where(p => p.IsPrivate && p.Creator.Username == username).ToListAsync();
    }

    public async Task<Playlist> GetPlaylistByIdAsync(Guid id)
    {
        return await _db.Playlists.Include(p => p.Creator).FirstOrDefaultAsync(t => t.Id == id);
    }

    public void CreatePlaylist(Playlist playlist)
    {
        _db.Add(playlist);
    }

    public void UpdatePlaylist(Playlist playlist)
    {
        _db.Update(playlist);
    }

    public void DeletePlaylist(Playlist playlist)
    {
        _db.Remove(playlist);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
