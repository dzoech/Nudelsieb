using System;
using System.Runtime.Serialization;

namespace Nudelsieb.Shared.Clients.Authentication
{
    [Serializable]
    internal class AuthenticationException : Exception
    {
        public AuthenticationException()
        {
        }

        public AuthenticationException(string message)
            : base(message)
        {
        }

        public AuthenticationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AuthenticationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
