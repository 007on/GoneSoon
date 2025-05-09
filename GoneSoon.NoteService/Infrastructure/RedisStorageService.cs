using GoneSoon.NoteService.Handlers;
using MassTransit.Mediator;
using StackExchange.Redis;

namespace GoneSoon.NoteService.Infrastructure
{
    public class RedisKeyExpirationWatcher : IDisposable
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly ISubscriber _redisSubscriber;
        private bool _isRunning;
        private readonly IMediator _mediator;
        private readonly ILogger<RedisKeyExpirationWatcher> _logger;

        /// <summary>
        /// Creates an instance of the class to track key expiration events in Redis.
        /// </summary>
        /// <param name="redisConnectionvar">Redis connection var.</param>
        public RedisKeyExpirationWatcher(string redisConnectionvar, IMediator mediator, ILogger<RedisKeyExpirationWatcher> logger)
        {
            _redisConnection = ConnectionMultiplexer.Connect(redisConnectionvar);
            _redisSubscriber = _redisConnection.GetSubscriber();
            _isRunning = false;
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Starts tracking key expiration events.
        /// </summary>
        public void StartWatching()
        {
            if (_isRunning)
            {
                _logger.LogWarning("Tracking is already running.");
                return;
            }

            var channel = "__keyevent@0__:expired"; // For database 0
            _redisSubscriber.Subscribe(channel, OnKeyExpired);

            _isRunning = true;
            _logger.LogInformation("Started tracking key expiration events...");
        }

        /// <summary>
        /// Stops tracking key expiration events.
        /// </summary>
        public void StopWatching()
        {
            if (!_isRunning)
            {
                _logger.LogWarning("Tracking is already stopped.");
                return;
            }

            var channel = "__keyevent@0__:expired"; // For database 0
            _redisSubscriber.Unsubscribe(channel);

            _isRunning = false;
            _logger.LogInformation("Stopped tracking key expiration events.");
        }

        /// <summary>
        /// Handles the key expiration event.
        /// </summary>
        /// <param name="channel">Redis channel.</param>
        /// <param name="message">The expired key name.</param>
        private void OnKeyExpired(RedisChannel channel, RedisValue message)
        {
            var expiredKey = message.ToString();
            _logger.LogInformation($"Key '{expiredKey}' has expired!");
            var noteId = RedisNoteRepositoryHelpers.GetNoteIdFromRedisKey(expiredKey);
            _mediator.Publish(new NoteExpiredNotification(noteId));
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        public void Dispose()
        {
            StopWatching();
            _redisConnection.Dispose();
        }
    }

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
