using MusicApp.Identity.Domain.Entities;

namespace MusicApp.Identity.Application.Repositories;

public interface IRoleRepository
{
    Role GetRoleByTitle(string title);
}
