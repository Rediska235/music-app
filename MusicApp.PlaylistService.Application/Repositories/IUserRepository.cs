using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Application.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
}
