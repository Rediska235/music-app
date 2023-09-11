namespace MusicApp.SongService.Application.Repositories;

public interface ISongMongoDbRepository
{
    Task LikeSongAsync(Guid id, string username);
    Task<IEnumerable<Guid>> GetFavoriteSongsAsync(string username);
    Task AddUser(string username);
}