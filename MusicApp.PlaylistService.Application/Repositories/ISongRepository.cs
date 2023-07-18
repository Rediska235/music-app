using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Application.Repositories;

public interface ISongRepository
{
    Task<Song> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
