using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Application.Services.Interfaces;

public interface IPlaylistsService
{
    Task<IEnumerable<Playlist>> GetPlaylists();
    Task<Playlist> GetPlaylistById(Guid id);
    Task CreatePlaylist(PlaylistInputDto playlist);
    Task UpdatePlaylist(Guid id, PlaylistInputDto playlist);
    Task DeletePlaylist(Guid id);
    Task AddSong(Guid playlistId, Guid songId);
    Task RemoveSong(Guid playlistId, Guid songId);
}
