using System;

namespace MWMS.Messaging.Infrastructure
{
    public class Event : Message
    {
        public int Priority { get; set; }
        public Event()
        {
        }

        public Event(Guid messageId) : base(messageId)
        {
        }

        public Event(string messageType) : base(messageType)
        {
        }

        public Event(Guid messageId, string messageType) : base(messageId, messageType)
        {
        }
    }
}
