using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.Services.Interfaces;

public interface IArtistService
{
    void ValidateArtistAndThrow(Song song);
    string GetUsername();
}
