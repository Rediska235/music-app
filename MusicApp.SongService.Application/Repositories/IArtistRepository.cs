using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.Repositories;

public interface IArtistRepository
{
    Task<Artist> GetArtistByUsernameAsync(string username, CancellationToken cancellationToken);
    Task CreateArtistAsync(Artist artist, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
