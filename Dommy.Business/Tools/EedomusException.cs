
namespace Dommy.Business.Tools
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class EedomusException : Exception
    {
        public EedomusException()
            : base()
        {

        }

        public EedomusException(string message)
            : base(message)
        {

        }

        public EedomusException(string message, Exception ex)
            : base(message, ex)
        {

        }

        protected EedomusException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {

        }
    }
}
