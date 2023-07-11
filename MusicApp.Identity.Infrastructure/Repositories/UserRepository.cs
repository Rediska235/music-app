using Microsoft.EntityFrameworkCore;
using MusicApp.Identity.Application.Repositories;
using MusicApp.Identity.Domain.Entities;
using MusicApp.Identity.Infrastructure.Data;

namespace MusicApp.Identity.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _db.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
    {
        return await _db.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }

    public async Task InsertUserAsync(User user)
    {
        await _db.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
