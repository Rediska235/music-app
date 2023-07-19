using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Application.Services.Interfaces;

public interface IPlaylistsService
{
    Task<IEnumerable<Playlist>> GetPlaylistsAsync(CancellationToken cancellationToken);
    Task<Playlist> GetPlaylistByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Playlist> CreatePlaylistAsync(PlaylistInputDto playlist, CancellationToken cancellationToken);
    Task<Playlist> UpdatePlaylistAsync(Guid id, PlaylistInputDto playlist, CancellationToken cancellationToken);
    Task DeletePlaylistAsync(Guid id, CancellationToken cancellationToken);
    Task AddSongAsync(Guid playlistId, Guid songId, CancellationToken cancellationToken);
    Task RemoveSongAsync(Guid playlistId, Guid songId, CancellationToken cancellationToken);
}
