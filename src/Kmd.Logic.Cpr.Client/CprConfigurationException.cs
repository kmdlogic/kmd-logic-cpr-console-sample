using System;
using System.Runtime.Serialization;

namespace Kmd.Logic.Cpr.Client
{
    [Serializable]
    public class CprConfigurationException : Exception
    {
        [Obsolete("This is no longer used and returns the Message.")]
        public string InnerMessage
        {
            get
            {
                return this.Message;
            }
        }

        public CprConfigurationException()
        {
        }

        public CprConfigurationException(string message)
            : base(message)
        {
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