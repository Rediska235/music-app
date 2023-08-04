using Microsoft.EntityFrameworkCore;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Infrastructure.Data;

namespace MusicApp.PlaylistService.Infrastructure.Repositories;

public class SongRepository : BaseRepository<Song>, ISongRepository
{
    private readonly AppDbContext _appContext;

    public SongRepository(AppDbContext appContext) : base(appContext)
    {
        _appContext = appContext;
    }
    
    public override async Task<Song?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _appContext.Songs
            .Include(song => song.Artist)
            .FirstOrDefaultAsync(song => song.Id == id, cancellationToken);
    }

    public async Task DeleteAllSongs(CancellationToken cancellationToken)
    {
        await _appContext.Songs.ExecuteDeleteAsync(cancellationToken);
    }
}
