using Microsoft.AspNetCore.Http;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Domain.Exceptions;

namespace MusicApp.SongService.Application;
public class ArtistService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ArtistService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void ValidateArtist(Song song)
    {
        var username = _httpContextAccessor.HttpContext.User.Identity.Name;
        
        if (song.Artist.Username != username)
        {
            throw CommonExceptions.notYourSong;
        }
    }
}
