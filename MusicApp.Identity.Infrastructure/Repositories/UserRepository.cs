using Microsoft.EntityFrameworkCore;
using MusicApp.Identity.Application.Repositories;
using MusicApp.Identity.Domain.Entities;
using MusicApp.Identity.Infrastructure.Data;

namespace MusicApp.Identity.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appContext;

    public UserRepository(AppDbContext appContext)
    {
        _appContext = appContext;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _appContext.Users.Include(user => user.Roles).FirstOrDefaultAsync(user => user.Username == username);
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        return await _appContext.Users.Include(user => user.Roles).FirstOrDefaultAsync(user => user.RefreshToken == refreshToken);
    }

    public async Task InsertUserAsync(User user)
    {
        await _appContext.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _appContext.SaveChangesAsync();
    }
}
