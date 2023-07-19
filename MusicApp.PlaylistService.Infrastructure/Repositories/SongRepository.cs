using Microsoft.EntityFrameworkCore;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Infrastructure.Data;

namespace MusicApp.PlaylistService.Infrastructure.Repositories;

public class SongRepository : ISongRepository
{
    private readonly AppDbContext _appContext;

    public SongRepository(AppDbContext appContext)
    {
        _appContext = appContext;
    }
    
    public async Task<Song> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _appContext.Songs
            .Include(s => s.Artist)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
}
