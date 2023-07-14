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

    public async Task<IEnumerable<Playlist>> GetPublicPlaylistsAsync(CancellationToken cancellationToken)
    {
        return await _db.Playlists
            .Include(p => p.Songs)
            .Include(p => p.Creator)
            .Where(p => !p.IsPrivate)
            .ToListAsync(cancellationToken);
    }
    public async Task<IEnumerable<Playlist>> GetMyPrivatePlaylistsAsync(string username, CancellationToken cancellationToken)
    {
        return await _db.Playlists
            .Include(p => p.Songs)
            .Include(p => p.Creator)
            .Where(p => p.IsPrivate && p.Creator.Username == username)
            .ToListAsync(cancellationToken);
    }

    public async Task<Playlist> GetPlaylistByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.Playlists
            .Include(p => p.Songs)
            .Include(p => p.Creator)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task CreatePlaylistAsync(Playlist playlist, CancellationToken cancellationToken)
    {
        await _db.AddAsync(playlist, cancellationToken);
    }

    public void UpdatePlaylist(Playlist playlist)
    {
        _db.Update(playlist);
    }

    public void DeletePlaylist(Playlist playlist)
    {
        _db.Remove(playlist);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}
