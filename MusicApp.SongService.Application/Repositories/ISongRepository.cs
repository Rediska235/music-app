using MusicApp.SongService.Domain.Entities;

namespace MusicApp.SongService.Application.Repositories;

public interface ISongRepository
{
    Task<IEnumerable<Song>> GetAllSongsAsync();
    Task<Song> GetSongByIdAsync(Guid id);
    void CreateSong(Song song);
    void UpdateSong(Song song);
    void DeleteSong(Song song);
    Task SaveChangesAsync();
}
