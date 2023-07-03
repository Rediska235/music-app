using MusicApp.SongService.Domain.Entities;
namespace MusicApp.SongService.Application.Services.Interfaces;

public interface ISongsService
{
    Task<IEnumerable<Song>> GetAllSongs();
    Task<Song> GetSongById(Guid id);
    Task CreateSong(Song song, string username);
    Task UpdateSong(Song song, string username);
    Task DeleteSong(Guid id, string username);
}
