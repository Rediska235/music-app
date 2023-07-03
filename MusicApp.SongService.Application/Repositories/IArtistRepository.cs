using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.Repositories;

public interface IArtistRepository
{
    Task<Artist> GetArtistbyUsername(string username);
    void CreateArtist(Artist artist);
    Task SaveChangesAsync();
}
