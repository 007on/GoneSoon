using System;
using System.Collections.Generic;

namespace GoneSoon.InteractionProtocol.NoteService.Data
{
    public class NewNoteDto
    {
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DeletingDate { get; set; }
        public List<NotificationRequestDto> NotificationMethods { get; set; } = new List<NotificationRequestDto>();
    }
}
