using StackExchange.Redis;
using TaskScheduler.Cache;

namespace TaskScheduler.Service.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redisConnection;

        public RedisCacheService(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
        }

        public async Task SetAsync(string key, string value, TimeSpan expiration)
        {
            var db = _redisConnection.GetDatabase();
            await db.StringSetAsync(key, value, expiration);
        }
        public async Task<string> GetAsync(string key)
        {
            var db = _redisConnection.GetDatabase();
            return await db.StringGetAsync(key);
        }
    }
}