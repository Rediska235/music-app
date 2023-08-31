using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Driver;
using MusicApp.SongService.Application.Repositories;
using MusicApp.SongService.Domain.Entities;
using MusicApp.SongService.Infrastructure.Data;
using MusicApp.SongService.Infrastructure.Extensions;

namespace MusicApp.SongService.Infrastructure.Repositories.MongoDb;

/**
 var db = client.GetDatabase("test");    // обращаемся к базе данных
    var collection = db.GetCollection<BsonDocument>("users"); // получаем коллекцию users
    // для теста добавляем начальные данные, если коллекция пуста
    if (await collection.CountDocumentsAsync("{}") == 0)
    {
        await collection.InsertManyAsync(new List<BsonDocument>
        {
            new BsonDocument{ { "Name", "Tom" },{"Age", 22}},
            new BsonDocument{ { "Name", "Bob" },{"Age", 42}}
        });
    }
    var users =  await collection.Find("{}").ToListAsync();
    return users.ToJson();  // отправляем клиенту все документы из коллекции
*/


public class SongRepository : ISongRepository
{
    protected MongoClient _mongoClient;
    protected IDistributedCache _cache;

    public SongRepository(MongoClient mongoClient, IDistributedCache cache)
    {
        _mongoClient = mongoClient;
        _cache = cache;

        var db = _mongoClient.GetDatabase("SongServiceDb");
        var collection = db.GetCollection<BsonDocument>("Songs");
    }

    public override async Task<IEnumerable<Song>> GetAsync(CancellationToken cancellationToken)
    {
        return await _appContext.Songs
            .Include(song => song.Artist)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public override async Task<Song?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var song = await _cache.GetEntityAsync<Song>(id.ToString(), cancellationToken);
        if (song != null)
        {
            return song;
        }

        song = await _appContext.Songs
            .Include(song => song.Artist)
            .AsNoTracking()
            .FirstOrDefaultAsync(song => song.Id == id, cancellationToken);

        if (song != null)
        {
            await _cache.SetEntityAsync(id.ToString(), song, cancellationToken);
        }

        return song;
    }
}
