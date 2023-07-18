using Microsoft.EntityFrameworkCore;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Infrastructure.Data;

namespace MusicApp.PlaylistService.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<User> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await _db.Users.FirstOrDefaultAsync(a => a.Username == username, cancellationToken);
    }
}
