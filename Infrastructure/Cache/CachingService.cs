using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Cache;
public class CachingService : ICachingService
{
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _options;
    private readonly IConfiguration _configuration;

    public CachingService(IDistributedCache cache)
    {
        _cache = cache;
        _options = new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(int.Parse(_configuration?["ExpirationDate"]??"24")),
            SlidingExpiration = TimeSpan.FromSeconds(1200)
        };
    }

    public async Task<string> GetAsync(string key) => await _cache.GetStringAsync(key);

    public async Task SetAsync(string key, string value) => await _cache.SetStringAsync(key, value, _options);
}
