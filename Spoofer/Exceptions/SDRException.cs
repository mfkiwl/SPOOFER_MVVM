using System;
using System.Runtime.Serialization;

namespace Spoofer.Services.Spoofer
{
    [Serializable]
    internal class SDRException : Exception
    {
        public override string Message => DEFAULT_MESSAGE;
        private const string DEFAULT_MESSAGE = "There is a problem with one or more of the Sdr Plugs, Please Fix It...";
        public SDRException()
        {
        }

        public SDRException(string message) : base(DEFAULT_MESSAGE)
        {
        }

        public SDRException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SDRException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}