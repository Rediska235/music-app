using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Infrastructure.Data;
using MusicApp.PlaylistService.Infrastructure.Extensions;

namespace MusicApp.PlaylistService.Infrastructure.Repositories;

public class SongRepository : BaseRepository<Song>, ISongRepository
{
    public SongRepository(AppDbContext appContext, IDistributedCache cache) : base(appContext, cache)
    {
        _appContext = appContext;
        _cache = cache;
    }

    public override async Task<Song?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var song = await _cache.GetEntityAsync<Song>(id.ToString(), cancellationToken);
        if (song != null)
        {
            return song;
        }

        song = await _appContext.Songs
            .Include(song => song.Artist)
            .FirstOrDefaultAsync(song => song.Id == id, cancellationToken);

        if (song != null)
        {
            await _cache.SetEntityAsync(id.ToString(), song, cancellationToken);
        }

        return song;
    }

    public async Task DeleteAllSongs(CancellationToken cancellationToken)
    {
        await _appContext.Songs.ExecuteDeleteAsync(cancellationToken);
    }
}
