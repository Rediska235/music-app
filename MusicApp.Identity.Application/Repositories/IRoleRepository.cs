using MusicApp.Identity.Domain.Entities;

namespace MusicApp.Identity.Application.Repositories;

public interface IRoleRepository
{
    Task<Role> GetRoleByTitleAsync(string title);
}
