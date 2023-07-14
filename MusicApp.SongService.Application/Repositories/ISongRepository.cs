using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.Repositories;

public interface ISongRepository
{
    Task<IEnumerable<Song>> GetSongsAsync(CancellationToken cancellationToken);
    Task<Song> GetSongByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateSongAsync(Song song, CancellationToken cancellationToken);
    void UpdateSong(Song song);
    void DeleteSong(Song song);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
