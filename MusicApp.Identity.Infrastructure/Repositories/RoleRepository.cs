using Microsoft.EntityFrameworkCore;
using MusicApp.Identity.Application.Repositories;
using MusicApp.Identity.Domain.Entities;
using MusicApp.Identity.Infrastructure.Data;

namespace MusicApp.Identity.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _appContext;

    public RoleRepository(AppDbContext appContext)
    {
        _appContext = appContext;
    }

    public async Task<Role> GetRoleByTitleAsync(string title)
    {
        return await _appContext.Roles.FirstOrDefaultAsync(role => role.Title == title);
    }
}
