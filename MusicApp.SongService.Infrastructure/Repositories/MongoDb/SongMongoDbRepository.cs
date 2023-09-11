using MongoDB.Bson;
using MongoDB.Driver;
using MusicApp.SongService.Application.Repositories;

namespace MusicApp.SongService.Infrastructure.Repositories.MongoDb;

public class SongMongoDbRepository : ISongMongoDbRepository
{
    const string FieldName = "liked-songs";

    protected MongoClient _mongoClient;
    protected IMongoCollection<BsonDocument> _collection;

    public SongMongoDbRepository(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;

        var db = _mongoClient.GetDatabase("SongServiceMongoDb");
        _collection = db.GetCollection<BsonDocument>("Songs");
    }

    public async Task LikeSongAsync(Guid songId, string username)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("username", username);
        var document = await _collection.Find(filter).FirstOrDefaultAsync();

        var songIds = document[FieldName].AsBsonArray.ToList();
        if (!songIds.Contains(songId.ToString()))
        {
            var update = Builders<BsonDocument>.Update.Push("liked-songs", songId.ToString());
            await _collection.UpdateOneAsync(filter, update);
        }
    }

    public async Task<IEnumerable<Guid>> GetFavoriteSongsAsync(string username)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("username", username);
        var document = await _collection.Find(filter).FirstOrDefaultAsync();
        if(document == null)
        {
            return null;
        }

        var songIds = document[FieldName].AsBsonArray.Values
            .Select(x => x.AsString)
            .ToList();

        return songIds.Select(Guid.Parse).ToList();
    }

    public async Task AddUser(string username)
    {
        await _collection.InsertOneAsync(new BsonDocument { { "username", username }, { "liked-songs", new BsonArray() } });
    }
}
