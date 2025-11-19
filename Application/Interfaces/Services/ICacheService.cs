namespace ShoesShop.Application.Interfaces.Services
{
    public interface ICacheService
    {
        Task<T> GetCacheAsync<T>(string key);
        Task SetCacheAsync<T>(string key, T value, TimeSpan slidingExp, TimeSpan absoluteExp);
        Task RemoveCacheAsync(string key);
    }

}
