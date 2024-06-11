using StackExchange.Redis;
using Simpl.Snippets.Service.Domain.CodeShare.Abstract;

namespace Simpl.Snippets.Service.Domain.CodeShare.Services
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _redis;

        public RedisService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _redis = _connectionMultiplexer.GetDatabase();
        }

        public async Task<string> GetMessageAsync(string key)
        {
            var value = await _redis.StringGetAsync(key);
            return value.HasValue ? value.ToString() : string.Empty;
        }

        public async Task SetMessageAsync(string key, string value, TimeSpan? expiry = null)
        {
            await _redis.StringSetAsync(key, value, expiry);
        }

        public async Task DeleteMessageAsync(string key)
        {
            await _redis.KeyDeleteAsync(key);
        }

        public async Task<string> CreateSessionAsync()
        {
            var sessionId = Guid.NewGuid().ToString();
            await SetMessageAsync(sessionId, "", TimeSpan.FromHours(24));
            return sessionId;
        }

        public async Task<bool> MessageExistsAsync(string key)
        {
            return await _redis.KeyExistsAsync(key);
        }

        public async Task<TimeSpan?> GetKeyTTLAsync(string key)
        {
            var ttl = await _redis.KeyTimeToLiveAsync(key);
            return ttl;
        }
    }
}
