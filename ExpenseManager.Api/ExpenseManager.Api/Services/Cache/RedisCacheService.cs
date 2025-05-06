using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System.Text.Json;
using ExpenseManager.Api.Services.Cashe;
using IDatabase = StackExchange.Redis.IDatabase;

namespace ExpenseManager.Api.Services.RedisCache;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _cacheDb;
    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _cacheDb = connectionMultiplexer.GetDatabase();
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var value = await _cacheDb.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return default;

        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _cacheDb.StringSetAsync(key, json, expiry);
    }

    public async Task RemoveAsync(string key)
    {
        await _cacheDb.KeyDeleteAsync(key);
    }
}
