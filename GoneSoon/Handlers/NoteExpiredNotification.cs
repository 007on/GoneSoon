using MediatR;

namespace GoneSoon.Handlers
{
    public record NoteExpiredNotification(Guid NoteId) : INotification;
}
