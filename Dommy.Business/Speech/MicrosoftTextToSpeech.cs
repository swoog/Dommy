using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Speech.Synthesis;
using Ninject.Extensions.Logging;
using Dommy.Business.Syntax;

namespace Dommy.Business.Speech
{
    public class MicrosoftTextToSpeech : ITextToSpeech
    {
        private SpeechSynthesizer synthesizer;

        private ILogger logger;

        public MicrosoftTextToSpeech(Gender gender, string culture, ILogger logger)
        {
            this.logger = logger;
            this.Culture = culture;
            this.Gender = gender;
            synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();

            var voices = synthesizer.GetInstalledVoices();

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

            synthesizer.SelectVoice(this.Language);
        }

        public Gender Gender { get; private set; }

        public string Culture { get; private set; }

        public string Language { get; set; }

        public void Speak(string text)
        {
            synthesizer.Speak(text);
        }
    }
}
