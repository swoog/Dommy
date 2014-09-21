//-----------------------------------------------------------------------
// <copyright file="SpeechLogger.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business
{
    using System;
    using System.Linq;
    using Ninject;
    using Ninject.Extensions.Logging;

    /// <summary>
    /// Actor of speech.
    /// </summary>
    public enum Actor
    {
        /// <summary>
        /// The actor of the speech is the speech recognized.
        /// </summary>
        Me,

        /// <summary>
        /// The actor is dommy.
        /// </summary>
        Dommy,
    }

    /// <summary>
    /// Speech logger.
    /// </summary>
    public class SpeechLogger : ISpeechLogger
    {
        /// <summary>
        /// Text to speech used.
        /// </summary>
        private ITextToSpeech textToSpeech;

        /// <summary>
        /// Last date time to speak.
        /// </summary>
        private DateTime lastSpeak;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeechLogger"/> class.
        /// </summary>
        /// <param name="logger">Logger errors.</param>
        /// <param name="textToSpeech">Text to speech.</param>
        public SpeechLogger(ILogger logger, ITextToSpeech textToSpeech)
        {
            this.Logger = logger;
            this.textToSpeech = textToSpeech;
        }

        /// <summary>
        /// Gets a value indicating whether the recognition must be ignored.
        /// </summary>
        /// <returns></returns>
        public bool IgnoreRecognition
        {
            get
            {
                lock (this)
                {
                    return DateTime.Now < this.lastSpeak.AddSeconds(1);
                }
            }
        }

        /// <summary>
        /// Gets the logger errors.
        /// </summary>
        /// <returns></returns>
        public ILogger Logger { get; private set; }

        /// <summary>
        /// Say message. If actor is dommy the text to speech is used.
        /// </summary>
        /// <param name="actor">Actor of the message.</param>
        /// <param name="message">Message to say.</param>
        public void Say(Actor actor, string message)
        {
            this.Logger.Info("{0} : {1}", actor, message);

            if (actor == Actor.Dommy)
            {
                this.Speak(message);
            }
        }

        /// <summary>
        /// Say an error..
        /// </summary>
        /// <param name="actor">Actor of the message.</param>
        /// <param name="errorMessage">Error message.</param>
        public void ErrorRecognition(Actor actor, string errorMessage)
        {
            this.Logger.Error("{0} : {1}", actor, errorMessage);

            if (actor == Actor.Dommy)
            {
                this.Speak(errorMessage);
            }
        }

        /// <summary>
        /// Used the text to speech to speak the text.
        /// </summary>
        /// <param name="text">Text to speak.</param>
        private void Speak(string text)
        {
            lock (this)
            {
                this.textToSpeech.Speak(text);
                this.lastSpeak = DateTime.Now;
            }
        }
    }
}
