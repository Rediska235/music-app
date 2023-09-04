namespace MusicApp.SongService.Application.Repositories;

public interface ISongMongoDbRepository
{
    Task LikeSong(Guid id, string username);
    Task<IEnumerable<Guid>> GetFavoriteSongs(string username);
}