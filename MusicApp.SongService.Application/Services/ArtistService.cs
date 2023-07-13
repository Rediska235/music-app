using Microsoft.AspNetCore.Http;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.SongService.Application.Services;

public class ArtistService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ArtistService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void ValidateArtistAndThrow(Song song)
    {
        var username = GetUsername();

        if (song.Artist.Username != username)
        {
            throw new NotYourSongException();
        }
    }

    public string GetUsername()
    {
        return _httpContextAccessor.HttpContext.User.Identity.Name;
    }
}
