using ShoesShop.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
namespace ShoesShop.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<T> GetCacheAsync<T>(string key)
        {
            var cachedData = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(cachedData))
                return default;

            return JsonSerializer.Deserialize<T>(cachedData);
        }


        public async Task RemoveCacheAsync(string key)
        {
             await _cache.RemoveAsync(key);
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan slidingExp, TimeSpan absoluteExp)
        {
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(slidingExp)
                .SetAbsoluteExpiration(absoluteExp);
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(value), options);
        }
    }
}
