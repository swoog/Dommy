//-----------------------------------------------------------------------
// <copyright file="SpeechLogger.cs" company="Trollcorp">
//     Copyright (c) agaltier, Trollcorp. All rights reserved.
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
        Me,
        Dommy,
    }

    /// <summary>
    /// Speech logger.
    /// </summary>
    public class SpeechLogger : ISpeechLogger
    {
        private ITextToSpeech textToSpeech;

        private DateTime lastSpeak;

        public SpeechLogger(ILogger logger, ITextToSpeech textToSpeech)
        {
            this.Logger = logger;
            this.textToSpeech = textToSpeech;
        }

        public bool IgnoreRecognition
        {
            get
            {
                lock (this)
                {
                    return DateTime.Now < lastSpeak.AddSeconds(1);
                }
            }
        }

        public ILogger Logger { get; private set; }

        public void Say(Actor actor, string message)
        {
            this.Logger.Info("{0} : {1}", actor, message);

            if (actor == Actor.Dommy)
            {
                Speak(message);
            }
        }

        private void Speak(string text)
        {
            lock (this)
            {
                this.textToSpeech.Speak(text);
                this.lastSpeak = DateTime.Now;
            }
        }

        public void ErrorRecognition(Actor actor, string errorMessage)
        {
            this.Logger.Error("{0} : {1}", actor, errorMessage);

            if (actor == Actor.Dommy)
            {
                Speak(errorMessage);
            }
        }
    }
}
