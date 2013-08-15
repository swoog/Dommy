//-----------------------------------------------------------------------
// <copyright file="SpeechLogger.cs" company="Microsoft">
//     Copyright (c) agaltier, Microsoft. All rights reserved.
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
    public class SpeechLogger
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
            get { return DateTime.Now < lastSpeak.AddSeconds(1); }
        }

        public ILogger Logger { get; private set; }

        public void Say(Actor actor, string text)
        {
            this.Logger.Info("{0} : {1}", actor, text);

            if (actor == Actor.Dommy)
            {
                Speak(text);
            }
        }

        private void Speak(string text)
        {
            this.textToSpeech.Speak(text);
            this.lastSpeak = DateTime.Now;
        }

        public void ErrorRecognition(Actor actor, string text)
        {
            this.Logger.Error("{0} : {1}", actor, text);

            if (actor == Actor.Dommy)
            {
                Speak(text);
            }
        }
    }
}
