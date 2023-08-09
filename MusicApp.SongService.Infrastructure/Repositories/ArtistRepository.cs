using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Infrastructure.Data;
using MusicApp.SongService.Infrastructure.Extensions;

namespace MusicApp.SongService.Infrastructure.Repositories;

public class ArtistRepository : BaseRepository<Artist>, IArtistRepository
{
    private readonly AppDbContext _appContext;
    private readonly IDistributedCache _cache;

    public ArtistRepository(AppDbContext appContext, IDistributedCache cache) : base(appContext, cache)
    {
        _appContext = appContext;
        _cache = cache;
    }

    public async Task<Artist?> GetArtistByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        var artist = await _cache.GetEntityAsync<Artist>(username, cancellationToken);
        if (artist != null)
        {
            _appContext.Entry(artist).State = EntityState.Unchanged;

            return artist;
        }

        artist = await _appContext.Artists.
            FirstOrDefaultAsync(artist => artist.Username == username, cancellationToken);

        if (artist != null)
        {
            await _cache.SetEntityAsync(username, artist, cancellationToken);
        }

        _appContext.Entry(artist).State = EntityState.Unchanged;

        return artist;
    }
}
