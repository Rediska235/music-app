using MusicApp.Identity.Application.Repositories;
using MusicApp.Identity.Domain.Entities;
using MusicApp.Identity.Infrastructure.Data;

namespace MusicApp.Identity.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _db;

    public RoleRepository(AppDbContext db)
    {
        _db = db;
    }

    public Role GetRoleByTitle(string title)
    {
        return _db.Roles.FirstOrDefault(r => r.Title == title);
    }
}
