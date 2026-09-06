using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Blog.Application.Caching;

// Written against IDistributedCache — .NET's own swappable cache
// abstraction — instead of a specific backing store. This class never
// changes when swapping in-process caching for real Redis; only the
// IDistributedCache registered underneath it does (see CachingRegistrar).
public class DistributedCacheService(IDistributedCache cache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var bytes = await cache.GetAsync(key, cancellationToken);
        return bytes is null ? default : JsonSerializer.Deserialize<T>(bytes);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
        var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration };
        return cache.SetAsync(key, bytes, options, cancellationToken);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        => cache.RemoveAsync(key, cancellationToken);
}
