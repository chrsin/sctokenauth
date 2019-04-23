using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace MyWebsite
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class AuthorizationException : Exception
    {
        public AuthorizationException(string message) : base(message)
        {
        }

        protected AuthorizationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}