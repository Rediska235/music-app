using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MusicApp.SongService.Infrastructure.Extensions;

public static class DistributedCacheExtension
{
    private static TimeSpan defaultExpireTime = TimeSpan.FromMinutes(1);

    public static async Task SetEntityAsync<T>(this IDistributedCache cache,
        string key,
        T data, 
        CancellationToken cancellationToken,
        TimeSpan? absoluteExpireTime = null,
        TimeSpan? unusedExpireTime = null)
    {
        var serializerOptions = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        var jsonData = JsonSerializer.Serialize(data, serializerOptions);

        var options = new DistributedCacheEntryOptions();

        options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? defaultExpireTime;
        options.SlidingExpiration = unusedExpireTime;

        await cache.SetStringAsync(key, jsonData, options, cancellationToken);
    }

    public static async Task<T?> GetEntityAsync<T>(this IDistributedCache cache, string key, CancellationToken cancellationToken)
    {
        var jsonData = await cache.GetStringAsync(key, cancellationToken);

        if(jsonData == null)
        {
            return default(T);
        }

        return JsonSerializer.Deserialize<T>(jsonData);
    }
}
