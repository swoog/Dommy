//-----------------------------------------------------------------------
// <copyright file="EedomusException.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Tools
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Eedomus exception.
    /// </summary>
    [Serializable]
    public class EedomusException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EedomusException"/> class.
        /// </summary>
        public EedomusException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EedomusException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public EedomusException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EedomusException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public EedomusException(string message, Exception ex)
            : base(message, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EedomusException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="streamingContext">The streaming context.</param>
        protected EedomusException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
