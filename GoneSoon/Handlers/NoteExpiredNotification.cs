using MediatR;

namespace GoneSoon.Handlers
{
    public record NoteExpiredNotification(Guid NoteId, string NoteTitle) : INotification;
}
