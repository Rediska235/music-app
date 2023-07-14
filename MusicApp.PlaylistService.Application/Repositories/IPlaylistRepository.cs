using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Application.Repositories;

public interface IPlaylistRepository
{
    Task<IEnumerable<Playlist>> GetPublicPlaylistsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Playlist>> GetMyPrivatePlaylistsAsync(string username, CancellationToken cancellationToken);
    Task<Playlist> GetPlaylistByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreatePlaylistAsync(Playlist playlist, CancellationToken cancellationToken);
    void UpdatePlaylist(Playlist playlist);
    void DeletePlaylist(Playlist playlist);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
