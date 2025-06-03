using System;
using System.Collections.Generic;

namespace GoneSoon.InteractionProtocol
{
    public class Note
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public string Content { get; set; }
        public DateTime ExpireDate { get; set; }
        public ICollection<NotificationMethod> NotificationMethodTypes { get; set; } = new HashSet<NotificationMethod>();
        public string Title { get; set; }
    }
}
