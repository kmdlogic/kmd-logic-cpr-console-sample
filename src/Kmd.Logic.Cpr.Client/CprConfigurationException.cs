using System;
using System.Runtime.Serialization;

namespace Kmd.Logic.Cpr.Client
{
    [Serializable]
    public class CprConfigurationException : Exception
    {
        public string InnerMessage { get; }

        public CprConfigurationException()
        {
        }

        public CprConfigurationException(string message)
            : base(message)
        {
        }

        public CprConfigurationException(string message, string innerMessage)
            : base(message)
        {
            this.InnerMessage = innerMessage;
        }

        public CprConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CprConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}