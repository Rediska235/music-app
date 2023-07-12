using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Application.Repositories;

public interface IPlaylistRepository
{
    Task<IEnumerable<Playlist>> GetPublicPlaylistsAsync();
    Task<IEnumerable<Playlist>> GetMyPrivatePlaylistsAsync(string username);
    Task<Playlist> GetPlaylistByIdAsync(Guid id);
    Task CreatePlaylistAsync(Playlist playlist);
    void UpdatePlaylist(Playlist playlist);
    void DeletePlaylist(Playlist playlist);
    Task SaveChangesAsync();
}
