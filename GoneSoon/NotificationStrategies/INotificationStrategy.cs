using GoneSoon.Models;

namespace GoneSoon.NotificationStrategies
{
    public interface INotificationStrategy
    {
        public NotificationMethod NotificationMethod { get; }

        public Task Notify(Note note);
    }

    public abstract class NotificationStrategyBase : INotificationStrategy
    {
        protected readonly ILogger _logger;

        protected NotificationStrategyBase(ILogger logger)
        {
            this._logger = logger;
        }

        public abstract NotificationMethod NotificationMethod { get; }

        public async Task Notify(Note note)
        {
            try
            {
                await NotifyInternal(note);
                _logger.LogInformation($"User {note.UserId} notified using {NotificationMethod}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to notify user {note.UserId} using {NotificationMethod}");
            }
        }

        protected abstract Task NotifyInternal(Note note);
    }

    public class EmailNotificationStrategy(ILogger logger) : NotificationStrategyBase(logger)
    {
        public override NotificationMethod NotificationMethod => NotificationMethod.Email;

        protected override Task NotifyInternal(Note note)
        {
            // Send email
            return Task.CompletedTask;
        }
    }

    public class SmsNotificationStrategy(ILogger logger) : NotificationStrategyBase(logger)
    {
        public override NotificationMethod NotificationMethod => NotificationMethod.Sms;

        protected override Task NotifyInternal(Note note)
        {
            // Send SMS
            return Task.CompletedTask;
        }
    }


    public class PushNotificationStrategy(ILogger logger) : NotificationStrategyBase(logger)
    {
        public override NotificationMethod NotificationMethod => NotificationMethod.Push;

        protected override Task NotifyInternal(Note note)
        {
            // Send push notification
            return Task.CompletedTask;
        }
    }
}
