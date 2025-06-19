using GoneSoon.InteractionProtocol.NoteService.Data;
using GoneSoon.NoteService.Domain;
using System.Reflection;

namespace GoneSoon.NoteService.NotificationStrategies
{
    public interface INotificationStrategy
    {
        public NotificationMethod NotificationMethod { get; }

        bool CanNotify { get; }

        public Task Notify(Notification notification);
    }

    public abstract class NotificationStrategyBase : INotificationStrategy
    {
        protected readonly ILogger _logger;

        protected NotificationStrategyBase(ILogger logger)
        {
            _logger = logger;
        }

        public abstract NotificationMethod NotificationMethod { get; }

        public abstract bool CanNotify { get; }

        public async Task Notify(Notification notification)
        {
            try
            {
                await NotifyInternal(notification);
                _logger.LogInformation($"User {notification.UserId} notified using {NotificationMethod}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to notify user {notification.UserId} using {NotificationMethod}");
            }
        }

        protected abstract Task NotifyInternal(Notification notification);
    }

    public class EmailNotificationStrategy(ILogger logger) : NotificationStrategyBase(logger)
    {
        public override NotificationMethod NotificationMethod => NotificationMethod.Email;

        public override bool CanNotify => true;

        protected override Task NotifyInternal(Notification notification)
        {
            // Send email
            return Task.CompletedTask;
        }
    }

    public class SmsNotificationStrategy(ILogger logger) : NotificationStrategyBase(logger)
    {
        public override NotificationMethod NotificationMethod => NotificationMethod.Sms;

        public override bool CanNotify => true;

        protected override Task NotifyInternal(Notification notification)
        {
            // Send SMS
            return Task.CompletedTask;
        }
    }


    public class PushNotificationStrategy(ILogger logger) : NotificationStrategyBase(logger)
    {
        public override NotificationMethod NotificationMethod => NotificationMethod.Push;

        public override bool CanNotify => true;

        protected override Task NotifyInternal(Notification notification)
        {
            // Send push notification
            return Task.CompletedTask;
        }
    }

    public interface INotificationStrategyFactory
    {
        INotificationStrategy GetNotificationStrategy(NotificationMethod method);
    }

    public class NotificationStrategyFactory : INotificationStrategyFactory
    {
        Dictionary<NotificationMethod, INotificationStrategy> strategies;

        public NotificationStrategyFactory(ILogger logger)
        {
            var strategyType = typeof(INotificationStrategy);
            strategies = Assembly.GetExecutingAssembly()
                                     .GetTypes()
                                     .Where(t => strategyType.IsAssignableFrom(t) && !t.IsAbstract)
                                     .Select(t => (INotificationStrategy)Activator.CreateInstance(t, logger))
                                     .Where(s => s.CanNotify)
                                     .ToDictionary(s => s.NotificationMethod);
            if (!strategies.Any())
            {
                throw new InvalidOperationException("No notification strategies found");
            }
        }

        public INotificationStrategy GetNotificationStrategy(NotificationMethod method)
        {
            return strategies.TryGetValue(method, out var strategy)
                ? strategy
                : throw new ArgumentException($"No strategy found for {method}");
        }
    }
}
