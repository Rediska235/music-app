using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Application.Repositories;

public interface IPlaylistRepository : IBaseRepository<Playlist>
{
    Task<IEnumerable<Playlist>> GetPublicPlaylistsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Playlist>> GetMyPrivatePlaylistsAsync(string username, CancellationToken cancellationToken);
}
