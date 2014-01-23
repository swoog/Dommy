using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Dommy.Business.Tools
{
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
