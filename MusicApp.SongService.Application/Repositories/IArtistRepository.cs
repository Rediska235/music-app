using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.Repositories;

public interface IArtistRepository
{
    Task<Artist> GetArtistByUsernameAsync(string username);
    Task CreateArtistAsync(Artist artist);
    Task SaveChangesAsync();
}
