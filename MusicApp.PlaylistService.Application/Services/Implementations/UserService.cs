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

    public void ValidateOwnerAndThrow(Playlist playlist)
    {
        var username = GetUsername();

        if (playlist.Creator.Username != username)
        {
            throw new NotYourPlaylistException();
        }
    }

    public async Task<User> GetOrCreateUser(CancellationToken cancellationToken)
    {
        var username = GetUsername();

        var user = await _repository.GetUserByUsernameAsync(username, cancellationToken);
        if (user != null)
        {
            return user;
        }

        user = new User()
        {
            Username = username
        };

        await _repository.CreateAsync(user, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        
        return user;
    }

    public string GetUsername()
    {
        return _httpContextAccessor.HttpContext.User.Identity.Name;
    }
}
