using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.Repositories;

public interface IArtistRepository : IBaseRepository<Artist>
{
    Task<Artist> GetArtistByUsernameAsync(string username, CancellationToken cancellationToken);
}
