using MusicApp.PlaylistService.Application.DTOs;

namespace MusicApp.PlaylistService.Application.Services.Interfaces;

public interface IPlaylistsService
{
    Task<IEnumerable<PlaylistOutputDto>> GetPlaylistsAsync(CancellationToken cancellationToken);
    Task<PlaylistOutputDto> GetPlaylistByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PlaylistOutputDto> CreatePlaylistAsync(PlaylistInputDto playlist, CancellationToken cancellationToken);
    Task<PlaylistOutputDto> UpdatePlaylistAsync(Guid id, PlaylistInputDto playlist, CancellationToken cancellationToken);
    Task DeletePlaylistAsync(Guid id, CancellationToken cancellationToken);
    Task AddSongAsync(Guid playlistId, Guid songId, CancellationToken cancellationToken);
    Task RemoveSongAsync(Guid playlistId, Guid songId, CancellationToken cancellationToken);
}
