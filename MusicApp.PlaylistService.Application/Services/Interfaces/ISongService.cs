using MusicApp.PlaylistService.Application.DTOs;
using MusicApp.PlaylistService.Domain.Entities;

namespace MusicApp.PlaylistService.Application.Services.Interfaces;

public interface ISongService
{
    Task<IEnumerable<SongOutputDto>> UpdateSongs(IEnumerable<Song> songs, CancellationToken cancellationToken);
}
