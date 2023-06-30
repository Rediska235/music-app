using MusicApp.Identity.Domain.Entities;

namespace MusicApp.Identity.Application.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User GetUserByUsername(string username);
    User GetUserByRefreshToken(string refreshToken);
    void InsertUser(User user);
    void UpdateUser(User user);
    void Save();
}
