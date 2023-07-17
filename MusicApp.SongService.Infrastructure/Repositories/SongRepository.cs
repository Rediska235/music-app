using Microsoft.EntityFrameworkCore;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Infrastructure.Data;

namespace MusicApp.SongService.Infrastructure.Repositories;

public class SongRepository : BaseRepository<Song>, ISongRepository
{
    private readonly AppDbContext _db;

    public SongRepository(AppDbContext db) : base(db)
    {
        _db = db;
    }

    public override async Task<IEnumerable<Song>> GetAsync(CancellationToken cancellationToken)
    {
        return await _db.Songs
            .Include(s => s.Artist)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public override async Task<Song> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.Songs
            .Include(s => s.Artist)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
}
