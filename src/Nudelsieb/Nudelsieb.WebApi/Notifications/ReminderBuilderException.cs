using System;
using System.Runtime.Serialization;

namespace Nudelsieb.WebApi.Notifications
{
    [Serializable]
    internal class ReminderBuilderException : Exception
    {
        public ReminderBuilderException()
        {
        }

        public ReminderBuilderException(string? message) : base(message)
        {
        }

        public ReminderBuilderException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ReminderBuilderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}