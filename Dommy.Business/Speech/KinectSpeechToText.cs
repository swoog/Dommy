//-----------------------------------------------------------------------
// <copyright file="KinectSpeechToText.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Speech
{
    using System;
    using System.Linq;
    using System.Threading;
    using Microsoft.Kinect;
    using Microsoft.Speech.AudioFormat;
    using Microsoft.Speech.Recognition;
    using Ninject.Extensions.Logging;
    
    public sealed class KinectSpeechToText : ISpeechToText, IDisposable
    {
        private SpeechRecognitionEngine speechRecognizer;

        private KinectSensor kinect = null;

        private ILogger logger;

        public string Culture { get; private set; }

        public KinectSpeechToText(string culture, ILogger logger)
        {
            this.logger = logger;
            this.Culture = culture;
        }

        private RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.OrdinalIgnoreCase)
                    && this.Culture.Equals(r.Culture.Name, StringComparison.OrdinalIgnoreCase)
                    ;
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }

        public void Init()
        {
            this.kinect = KinectSensor.KinectSensors.FirstOrDefault();
            if (this.kinect == null)
            {
                this.logger.Error("Kinect sensor not found.");
                this.logger.Info("Used mic input.");
            }
            else
            {
                if (!this.kinect.IsRunning)
                {
                    this.logger.Info("Sensor found");
                    while (this.kinect.Status == KinectStatus.Initializing)
                    {
                        this.logger.Info("Initializing...");
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                    }
                    this.kinect.Start();
                }

                this.logger.Info("Sensor initialized");
            }

            RecognizerInfo ri = GetKinectRecognizer();

            if (ri == null)
            {
                this.logger.Error("There was a problem initializing Speech Recognition. Ensure you have the Microsoft Speech SDK installed.");
                return;
            }

            try
            {
                speechRecognizer = new SpeechRecognitionEngine(ri.Id);
            }
            catch
            {
                this.logger.Error("There was a problem initializing Speech Recognition. Ensure you have the Microsoft Speech SDK installed.");
            }

            if (speechRecognizer == null)
            {
                return;
            }
        }

        public void Start(Action<ISentence> action)
        {
            if (this.speechRecognizer != null)
            {
                speechRecognizer.SpeechRecognized += (o, e) =>
                {
                    action(new Sentence
                    {
                        Words = e.Result.Words.Select(w => w.Text).ToArray(),
                        WordsConfidence = e.Result.Words.Select(w => w.Confidence).ToArray(),
                        Text = e.Result.Text,
                        Confidence = e.Result.Confidence,
                    });
                };
                speechRecognizer.SpeechHypothesized += speechRecognizer_SpeechHypothesized;
                speechRecognizer.SpeechRecognitionRejected += speechRecognizer_SpeechRecognitionRejected;
                this.logger.Info("Subscribe to recognized event");

                if (this.kinect != null)
                {
                    var audioSource = this.kinect.AudioSource;
                    //audioSource.BeamAngleMode = BeamAngleMode.Adaptive;
                    var audioStream = audioSource.Start();
                    speechRecognizer.MaxAlternates = 2;
                    speechRecognizer.UpdateRecognizerSetting("AdaptationOn", 0);
                    speechRecognizer.SetInputToAudioStream(audioStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                }
                else
                {
                    speechRecognizer.SetInputToDefaultAudioDevice();
                }

                speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        private void speechRecognizer_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            this.logger.Debug("Reject : {0}", e.Result.Text);
        }

        private void speechRecognizer_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            this.logger.Debug("Hypothesized : {0} {1:0.##}", e.Result.Text, e.Result.Confidence);
        }

        public void LoadGrammar(Grammar grammar)
        {
            if (this.speechRecognizer != null)
            {
                this.speechRecognizer.LoadGrammar(grammar);
            }
        }

        public void UnloadGrammar(Grammar grammar)
        {
            if (this.speechRecognizer != null)
            {
                this.speechRecognizer.UnloadGrammar(grammar);
            }
        }

        public void Stop()
        {
            if (this.kinect != null && this.kinect.IsRunning)
            {
                this.logger.Info("Sensor stoping...");
                this.kinect.Stop();
                this.logger.Info("Sensor stoped");
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected void Dispose(bool b)
        {
            if (this.kinect != null)
            {
                this.kinect.Dispose();
            }

            if (this.speechRecognizer != null)
            {
                this.speechRecognizer.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
