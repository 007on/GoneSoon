using GoneSoon.Handlers;
using MediatR;
using StackExchange.Redis;

namespace GoneSoon.Infrastructure
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
}
