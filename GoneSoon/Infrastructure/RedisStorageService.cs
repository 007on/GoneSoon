using StackExchange.Redis;

namespace GoneSoon.Infrastructure
{
    public class RedisStorageService : IRedisStorageService
    {
        private readonly IDatabase _db;
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisStorageService> _logger;

        public RedisStorageService(IConnectionMultiplexer redis, ILogger<RedisStorageService> logger)
        {
            _redis = redis;
            _db = redis.GetDatabase();
            _logger = logger;
        }

        public async Task SetAsync(string key, string value, TimeSpan? expiry = null) =>
            await _db.StringSetAsync(key, value, expiry);

        public async Task<string?> GetAsync(string key) =>
            await _db.StringGetAsync(key);

        public async Task<bool> DeleteAsync(string key) =>
            await _db.KeyDeleteAsync(key);

        public async Task<TimeSpan?> GetTtlAsync(string key) =>
            await _db.KeyTimeToLiveAsync(key);

        public async Task<IEnumerable<string>> GetKeysAsync(string pattern)
        {
            var server = _redis.GetServer(_redis.GetEndPoints()[0]);
            return server.Keys(pattern: pattern).Select(k => k.ToString());
        }
    }


}
