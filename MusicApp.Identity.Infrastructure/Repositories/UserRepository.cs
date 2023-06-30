using Microsoft.EntityFrameworkCore;
using MusicApp.Identity.Application.Repositories;
using MusicApp.Identity.Infrastructure.Data;
using MusicApp.Identity.Domain.Entities;

namespace MusicApp.Identity.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _db.Users.Include(u => u.Roles);
    }

    public User GetUserByUsername(string username)
    {
        return _db.Users.Include(u => u.Roles).FirstOrDefault(u => u.Username == username);
    }

    public User GetUserByRefreshToken(string refreshToken)
    {
        return _db.Users.Include(u => u.Roles).FirstOrDefault(u => u.RefreshToken == refreshToken);
    }

    public void InsertUser(User user)
    {
        _db.Add(user);
    }

    public void UpdateUser(User user)
    {
        _db.Update(user);
    }

    public void Save()
    {
        _db.SaveChanges();
    }
}
