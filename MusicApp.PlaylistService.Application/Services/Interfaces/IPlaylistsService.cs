using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Application.Services.Interfaces;

public interface IPlaylistsService
{
    Task<IEnumerable<Playlist>> GetPlaylists();
    Task<Playlist> GetPlaylistById(Guid id);
    Task CreatePlaylist(Playlist playlist);
    Task UpdatePlaylist(Playlist playlist);
    Task DeletePlaylist(Guid id);
    Task AddSong(Guid playlistId, Guid songId);
    Task RemoveSong(Guid playlistId, Guid songId);
}
