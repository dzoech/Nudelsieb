using System;
using System.Runtime.Serialization;

namespace Nudelsieb.Notifications.Notifyer
{
    [Serializable]
    internal class NotifyerException : Exception
    {
        public NotifyerException()
        {
        }

        public NotifyerException(string? message) : base(message)
        {
        }

        public NotifyerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NotifyerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}