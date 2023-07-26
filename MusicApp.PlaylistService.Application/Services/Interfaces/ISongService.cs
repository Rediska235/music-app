using MusicApp.PlaylistService.Domain.Entities;
using MusicApp.Shared;

namespace MusicApp.PlaylistService.Application.Services.Interfaces;

public interface ISongService
{
    Task<Song> AddSongAsync(SongPublishedDto songPublishedDto, CancellationToken cancellationToken);
    Task<Song> UpdateSongAsync(SongPublishedDto songPublishedDto, CancellationToken cancellationToken);
    Task RemoveSongAsync(SongPublishedDto songPublishedDto, CancellationToken cancellationToken);
}
