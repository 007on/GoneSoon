using System.ComponentModel.DataAnnotations;

namespace GoneSoon.InteractionProtocol
{
    public class NotificationRequestDto
    {
        [Required]
        public NotificationMethod Method { get; set; }  // Email, Sms, Push и т.д.

        [Required, MinLength(3)]
        public string Value { get; set; }               // Email address, phone number, token, etc.
    }
}
