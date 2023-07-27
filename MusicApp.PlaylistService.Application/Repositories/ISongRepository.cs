using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Application.Repositories;

public interface ISongRepository : IBaseRepository<Song>
{
    Task DeleteAllSongs(CancellationToken cancellationToken);
}
