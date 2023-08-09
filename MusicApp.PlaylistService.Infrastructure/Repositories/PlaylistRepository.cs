using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Infrastructure.Data;
using MusicApp.PlaylistService.Infrastructure.Extensions;

namespace MusicApp.PlaylistService.Infrastructure.Repositories;

public class PlaylistRepository : BaseRepository<Playlist>,  IPlaylistRepository
{
    private readonly AppDbContext _appContext;
    private readonly IDistributedCache _cache;

    public PlaylistRepository(AppDbContext appContext, IDistributedCache cache) : base(appContext, cache)
    {
        _appContext = appContext;
        _cache = cache;
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

    public override async Task<Playlist?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var playlist = await _cache.GetEntityAsync<Playlist>(id.ToString(), cancellationToken);
        if (playlist != null)
        {
            return playlist;
        }

        playlist = await _appContext.Playlists
            .Include(playlist => playlist.Songs)
            .Include(playlist => playlist.Creator)
            .FirstOrDefaultAsync(playlist => playlist.Id == id, cancellationToken);

        if (playlist != null)
        {
            await _cache.SetEntityAsync(id.ToString(), playlist, cancellationToken);
        }

        return playlist;
    }
}
