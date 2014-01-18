//-----------------------------------------------------------------------
// <copyright file="ISpeechLogger.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface used for speech.
    /// </summary>
    public interface ISpeechLogger
    {
        /// <summary>
        /// Gets a value indicating whether ignore recognition when dommy is saying.
        /// </summary>
        bool IgnoreRecognition { get; }

        /// <summary>
        /// Speech the error.
        /// </summary>
        /// <param name="actor">Actor saying the error.</param>
        /// <param name="errorMessage">Error message.</param>
        void ErrorRecognition(Actor actor, string errorMessage);

        /// <summary>
        /// Speech a message.
        /// </summary>
        /// <param name="actor">Actor saying the message.</param>
        /// <param name="message">Message to say.</param>
        void Say(Actor actor, string message);
    }
}
