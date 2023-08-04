using Microsoft.AspNetCore.Http;
using MusicApp.SongService.Application.Services.Interfaces;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.SongService.Application.Services.Implementations;

public class ArtistService : IArtistService
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
