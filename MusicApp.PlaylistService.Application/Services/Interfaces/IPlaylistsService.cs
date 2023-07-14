using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Application.Services.Interfaces;

public interface IPlaylistsService
{
    Task<IEnumerable<Playlist>> GetPlaylists(CancellationToken cancellationToken);
    Task<Playlist> GetPlaylistById(Guid id, CancellationToken cancellationToken);
    Task CreatePlaylist(PlaylistInputDto playlist, CancellationToken cancellationToken);
    Task UpdatePlaylist(Guid id, PlaylistInputDto playlist, CancellationToken cancellationToken);
    Task DeletePlaylist(Guid id, CancellationToken cancellationToken);
    Task AddSong(Guid playlistId, Guid songId, CancellationToken cancellationToken);
    Task RemoveSong(Guid playlistId, Guid songId, CancellationToken cancellationToken);
}
