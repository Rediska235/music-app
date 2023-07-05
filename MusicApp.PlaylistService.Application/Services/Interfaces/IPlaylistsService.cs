using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Application.Services.Interfaces;

public interface IPlaylistsService
{
    Task<IEnumerable<Playlist>> GetAllPlaylists(string username);
    Task<Playlist> GetPlaylistById(Guid id, string username);
    Task CreatePlaylist(Playlist playlist, string username);
    Task UpdatePlaylist(Playlist playlist, string username);
    Task DeletePlaylist(Guid id, string username);
    Task AddSong(Guid playlistId, Guid songId, string username);
    Task RemoveSong(Guid playlistId, Guid songId, string username);
}
