using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Application.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByUsername(string username);
    Task CreateUserAsync(User user);
    Task SaveChangesAsync();
}
