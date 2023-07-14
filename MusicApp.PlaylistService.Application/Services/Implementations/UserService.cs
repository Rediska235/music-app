using Microsoft.AspNetCore.Http;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Domain.Exceptions;

namespace MusicApp.PlaylistService.Application.Services.Implementations;

public class UserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _repository;

    public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository repository)
    {
        _httpContextAccessor = httpContextAccessor;
        _repository = repository;
    }

    public void ValidateOwner(Playlist playlist)
    {
        var username = GetUsername();

        if (playlist.Creator.Username != username)
        {
            throw new NotYourPlaylistException();
        }
    }

    public async Task<User> GetOrCreateUser()
    {
        var username = GetUsername();

        var user = await _repository.GetUserByUsername(username);
        if (user != null)
        {
            return user;
        }

        user = new User()
        {
            Username = username
        };

        await _repository.CreateUserAsync(user);
        await _repository.SaveChangesAsync();
        
        return user;
    }

    public string GetUsername()
    {
        return _httpContextAccessor.HttpContext.User.Identity.Name;
    }
}
