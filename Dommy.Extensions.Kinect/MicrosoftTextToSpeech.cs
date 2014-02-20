//-----------------------------------------------------------------------
// <copyright file="MicrosoftTextToSpeech.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.Kinect
{
    using Dommy.Business;
    using Dommy.Business.Syntax;
    using Microsoft.Speech.Synthesis;
    using Ninject.Extensions.Logging;
    using System;
    using System.Linq;

    /// <summary>
    /// Implement a text to speech.
    /// </summary>
    public sealed class MicrosoftTextToSpeech : ITextToSpeech, IDisposable
    {
        /// <summary>
        /// Speech synthesizer.
        /// </summary>
        private SpeechSynthesizer synthesizer;

        /// <summary>
        /// Logger information.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftTextToSpeech"/> class.
        /// </summary>
        /// <param name="gender">Gender speech.</param>
        /// <param name="culture">Culture accent.</param>
        /// <param name="logger">Logger information.</param>
        public MicrosoftTextToSpeech(Gender gender, string culture, ILogger logger)
        {
            this.logger = logger;
            this.Culture = culture;
            this.Gender = gender;

            try
            {
                this.synthesizer = new SpeechSynthesizer();
                this.synthesizer.SetOutputToDefaultAudioDevice();

                var voices = this.synthesizer.GetInstalledVoices();

                foreach (var lang in voices.Select(v => v.VoiceInfo.Name).ToArray())
                {
                    this.logger.Info("Lang : {0}", lang);
                }

                var voice = voices.FirstOrDefault(v => v.VoiceInfo.Gender == (this.Gender == Gender.Female ? VoiceGender.Female : VoiceGender.Male) && v.VoiceInfo.Culture.Name == this.Culture);
                if (voice == null)
                {
                    this.logger.Error("Not found {0} {1}", gender, culture);
                    voice = voices.FirstOrDefault();
                    this.Language = voice.VoiceInfo.Name;
                    this.logger.Info("Used lang : {0}", this.Language);
                }
                else
                {
                    this.Language = voice.VoiceInfo.Name;
                    this.logger.Info("Selected lang : {0}", this.Language);
                }

                this.synthesizer.SelectVoice(this.Language);
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "Error to initialize speech to text.");
                this.synthesizer = null;
            }
        }

        /// <summary>
        /// Gets gender speech.
        /// </summary>
        public Gender Gender { get; private set; }

        /// <summary>
        /// Gets culture speech.
        /// </summary>
        public string Culture { get; private set; }

        /// <summary>
        /// Gets language name.
        /// </summary>
        public string Language { get; private set; }

        /// <summary>
        /// Speak text.
        /// </summary>
        /// <param name="text">Text to speak.</param>
        public void Speak(string text)
        {
            if (this.synthesizer != null)
            {
                this.synthesizer.Speak(text);
            }
        }

        /// <summary>
        /// Dispose synthesizer.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Dispose synthesizer.
        /// </summary>
        /// <param name="b">Indicate dispose finalize.</param>
        public void Dispose(bool b)
        {
            if (this.synthesizer != null)
            {
                this.synthesizer.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}