using Microsoft.EntityFrameworkCore;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Infrastructure.Data;

namespace MusicApp.PlaylistService.Infrastructure.Repositories;

public class PlaylistRepository : BaseRepository<Playlist>,  IPlaylistRepository
{
    private readonly AppDbContext _appContext;

    public PlaylistRepository(AppDbContext appContext) : base(appContext)
    {
        _appContext = appContext;
    }

    public async Task<IEnumerable<Playlist>> GetPublicPlaylistsAsync(CancellationToken cancellationToken)
    {
        return await _appContext.Playlists
            .Include(playlist => playlist.Songs)
            .Include(playlist => playlist.Creator)
            .Where(playlist => !playlist.IsPrivate)
            .ToListAsync(cancellationToken);
    }
    public async Task<IEnumerable<Playlist>> GetMyPrivatePlaylistsAsync(string username, CancellationToken cancellationToken)
    {
        return await _appContext.Playlists
            .Include(playlist => playlist.Songs)
            .Include(playlist => playlist.Creator)
            .Where(playlist => playlist.IsPrivate && playlist.Creator.Username == username)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Playlist> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _appContext.Playlists
            .Include(playlist => playlist.Songs)
            .Include(playlist => playlist.Creator)
            .FirstOrDefaultAsync(playlist => playlist.Id == id, cancellationToken);
    }
}
