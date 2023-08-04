using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.Shared;

namespace MusicApp.PlaylistService.Application.Services.Interfaces;

public interface IUserService
{
    Task<User> GetUserAsync(CancellationToken cancellationToken);
    Task<User> AddUserAsync(UserPublishedDto userPublishedDto, CancellationToken cancellationToken);
    void ValidateOwnerAndThrow(Playlist playlist);
    string GetUsername();
}
