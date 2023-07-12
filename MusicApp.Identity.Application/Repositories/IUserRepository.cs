using MusicApp.Identity.Domain.Entities;

namespace MusicApp.Identity.Application.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByUsernameAsync(string username);
    Task<User> GetUserByRefreshTokenAsync(string refreshToken);
    Task InsertUserAsync(User user);
    Task SaveChangesAsync();
}
