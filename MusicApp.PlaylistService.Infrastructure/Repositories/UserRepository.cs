using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Infrastructure.Data;
using MusicApp.PlaylistService.Infrastructure.Extensions;

namespace MusicApp.PlaylistService.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext appContext, IDistributedCache cache) : base(appContext, cache)
    {
        _appContext = appContext;
        _cache = cache;
    }

    public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        var artist = await _cache.GetEntityAsync<User>(username, cancellationToken);
        if (artist != null)
        {
            _appContext.Entry(artist).State = EntityState.Unchanged;

            return artist;
        }

        artist = await _appContext.Users.
            FirstOrDefaultAsync(user => user.Username == username, cancellationToken);

        if (artist != null)
        {
            await _cache.SetEntityAsync(username, artist, cancellationToken);
        }

        _appContext.Entry(artist).State = EntityState.Unchanged;

        return artist;
    }
}
