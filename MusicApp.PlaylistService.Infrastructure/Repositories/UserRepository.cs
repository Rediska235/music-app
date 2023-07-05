using Microsoft.EntityFrameworkCore;
using MusicApp.PlaylistService.Application.Repositories;
using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.PlaylistService.Infrastructure.Data;

namespace MusicApp.PlaylistService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User> GetUserByUsername(string username)
    {
        return await _db.Users.FirstOrDefaultAsync(a => a.Username == username);
    }

    public void CreateUser(User user)
    {
        _db.Add(user);
    }
}
