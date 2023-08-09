using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Infrastructure.Data;
using MusicApp.SongService.Infrastructure.Extensions;

namespace MusicApp.SongService.Infrastructure.Repositories;

public class SongRepository : BaseRepository<Song>, ISongRepository
{
    private readonly AppDbContext _appContext;
    private readonly IDistributedCache _cache;

    public SongRepository(AppDbContext appContext, IDistributedCache cache) : base(appContext, cache)
    {
        _appContext = appContext;
        _cache = cache;
    }

    public override async Task<IEnumerable<Song>> GetAsync(CancellationToken cancellationToken)
    {
        return await _appContext.Songs
            .Include(song => song.Artist)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public override async Task<Song?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var song = await _cache.GetEntityAsync<Song>(id.ToString(), cancellationToken);
        if(song != null) 
        {
            return song;
        }

        song = await _appContext.Songs
            .Include(song => song.Artist)
            .AsNoTracking()
            .FirstOrDefaultAsync(song => song.Id == id, cancellationToken);

        if(song != null)
        {
            await _cache.SetEntityAsync(id.ToString(), song, cancellationToken);
        }

        return song;
    }
}
