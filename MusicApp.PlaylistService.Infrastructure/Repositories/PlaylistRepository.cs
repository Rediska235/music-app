using Microsoft.EntityFrameworkCore;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Infrastructure.Data;

namespace MusicApp.PlaylistService.Infrastructure.Repositories;

public class PlaylistRepository : BaseRepository<Playlist>,  IPlaylistRepository
{
    private readonly AppDbContext _db;

    public PlaylistRepository(AppDbContext db) : base(db)
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

    public override async Task<Playlist> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.Playlists
            .Include(p => p.Songs)
            .Include(p => p.Creator)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}
