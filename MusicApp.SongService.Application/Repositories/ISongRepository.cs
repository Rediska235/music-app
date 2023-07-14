using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.Repositories;

public interface ISongRepository
{
    Task<IEnumerable<Song>> GetSongsAsync();
    Task<Song> GetSongByIdAsync(Guid id);
    Task CreateSongAsync(Song song);
    void UpdateSong(Song song);
    void DeleteSong(Song song);
    Task SaveChangesAsync();
}
