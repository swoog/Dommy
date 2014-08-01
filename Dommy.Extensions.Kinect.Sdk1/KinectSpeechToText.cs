//-----------------------------------------------------------------------
// <copyright file="KinectSpeechToText.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.Kinect.Sdk1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Dommy.Business;
    using Microsoft.Kinect;
    using Microsoft.Speech.AudioFormat;
    using Microsoft.Speech.Recognition;
    using Ninject.Extensions.Logging;

    /// <summary>
    /// Implement Speech to text with Kinect.
    /// </summary>
    public sealed class KinectSpeechToText : ISpeechToText, IDisposable
    {
        /// <summary>
        /// Instance of the speech recognizer.
        /// </summary>
        private SpeechRecognitionEngine speechRecognizer;

        /// <summary>
        /// Instance of the kinect sensor selector.
        /// </summary>
        private KinectSensorSelector kinect = null;

        /// <summary>
        /// Instance of a logger.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Grammar information.
        /// </summary>
        private Dictionary<GrammarData, Grammar> grammars;

        /// <summary>
        /// Initializes a new instance of the <see cref="KinectSpeechToText"/> class.
        /// </summary>
        /// <param name="culture">Culture to recognize.</param>
        /// <param name="kinect">Kinect sensor selector.</param>
        /// <param name="logger">Instance of a logger.</param>
        public KinectSpeechToText(string culture, KinectSensorSelector kinect, ILogger logger)
        {
            this.logger = logger;
            this.Culture = culture;
            this.grammars = new Dictionary<GrammarData, Grammar>();
            this.kinect = kinect;
        }

        /// <summary>
        /// Search for the kinect recognizer.
        /// </summary>
        /// <returns>Return recognizer info.</returns>
        private RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.OrdinalIgnoreCase)
                    && this.Culture.Equals(r.Culture.Name, StringComparison.OrdinalIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }

        /// <summary>
        /// Rejected recognition.
        /// </summary>
        /// <param name="sender">Sender of the recognition.</param>
        /// <param name="e">Recognition information.</param>
        private void SpeechRecognizer_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            this.logger.Debug("Reject : {0}", e.Result.Text);
        }

        /// <summary>
        /// Recognition hypothesized.
        /// </summary>
        /// <param name="sender">Sender of the recognition.</param>
        /// <param name="e">Recognition information.</param>
        private void SpeechRecognizer_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            this.logger.Debug("Hypothesized : {0} {1:0.##}", e.Result.Text, e.Result.Confidence);
        }

        /// <summary>
        /// Create a Microsoft grammar object from grammar data.
        /// </summary>
        /// <param name="data">Grammar data.</param>
        /// <returns>Return a Microsoft grammar object.</returns>
        private Grammar CreateGrammar(GrammarData data)
        {
            GrammarBuilder gb = new GrammarBuilder();
            foreach (var choice in data.Choices)
            {
                var c = new Choices();
                foreach (var element in choice.Elements)
                {
                    c.Add(element);
                }

                gb.Append(c);
            }

            return new Grammar(gb);
        }

        /// <summary>
        /// Gets culture to recognize.
        /// </summary>
        public string Culture { get; private set; }

        public bool IsActive { get; private set; }

        /// <summary>
        /// Initialize speech recognizer.
        /// </summary>
        public void Init()
        {
            if (!this.kinect.IsKinectFound)
            {
                this.logger.Error("Kinect sensor not found.");
                this.logger.Info("Used mic input.");
                this.IsActive = false;
            }
            else
            {
                this.kinect.Start();
                this.IsActive = true;
            }

            RecognizerInfo ri = this.GetKinectRecognizer();

            if (ri == null)
            {
                this.logger.Error("There was a problem initializing Speech Recognition. Ensure you have the Microsoft Speech SDK installed.");
                return;
            }

            try
            {
                this.speechRecognizer = new SpeechRecognitionEngine(ri.Id);
            }
            catch
            {
                this.logger.Error("There was a problem initializing Speech Recognition. Ensure you have the Microsoft Speech SDK installed.");
            }

            if (this.speechRecognizer == null)
            {
                return;
            }
        }

        /// <summary>
        /// Start recognition.
        /// </summary>
        /// <param name="action">Action to execute when recognition is complete.</param>
        public void Start(Action<ISentence> action)
        {
            if (this.speechRecognizer != null)
            {
                this.speechRecognizer.SpeechRecognized += (o, e) =>
                {
                    action(new Sentence
                    {
                        Words = e.Result.Words.Select(w => w.Text).ToArray(),
                        WordsConfidence = e.Result.Words.Select(w => w.Confidence).ToArray(),
                        Text = e.Result.Text,
                        Confidence = e.Result.Confidence,
                    });
                };
                this.speechRecognizer.SpeechHypothesized += this.SpeechRecognizer_SpeechHypothesized;
                this.speechRecognizer.SpeechRecognitionRejected += this.SpeechRecognizer_SpeechRecognitionRejected;
                this.logger.Info("Subscribe to recognized event");

                if (this.kinect.IsKinectFound)
                {
                    var audioSource = this.kinect.Sensor.AudioSource;

                    audioSource.BeamAngleMode = BeamAngleMode.Manual;
                    audioSource.ManualBeamAngle = 0;
                    var audioStream = audioSource.Start();
                    this.speechRecognizer.MaxAlternates = 2;
                    this.speechRecognizer.UpdateRecognizerSetting("AdaptationOn", 0);
                    this.speechRecognizer.SetInputToAudioStream(audioStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                }
                else
                {
                    this.speechRecognizer.SetInputToDefaultAudioDevice();
                }

                this.speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        /// <summary>
        /// Load grammar to the recognizer engine.
        /// </summary>
        /// <param name="grammar">Grammar to load.</param>
        public void LoadGrammar(GrammarData grammar)
        {
            if (this.speechRecognizer != null)
            {
                Grammar g = null;
                if (this.grammars.ContainsKey(grammar))
                {
                    g = this.grammars[grammar];
                }
                else
                {
                    g = this.CreateGrammar(grammar);
                    this.grammars.Add(grammar, g);
                }

                this.speechRecognizer.LoadGrammar(g);
            }
        }

        /// <summary>
        /// Unload grammar to the recognizer engine.
        /// </summary>
        /// <param name="grammar">Grammar to unload.</param>
        public void UnloadGrammar(GrammarData grammar)
        {
            if (this.speechRecognizer != null)
            {
                Grammar g = null;
                if (this.grammars.ContainsKey(grammar))
                {
                    g = this.grammars[grammar];
                }
                else
                {
                    g = this.CreateGrammar(grammar);
                    this.grammars.Add(grammar, g);
                }

                this.speechRecognizer.UnloadGrammar(g);
            }
        }

        /// <summary>
        /// Stop recognizer.
        /// </summary>
        public void Stop()
        {
            this.kinect.Stop();
        }

        /// <summary>
        /// Dispose recognizer.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Dispose recognizer.
        /// </summary>
        /// <param name="b">Indicate dispose finalize.</param>
        public void Dispose(bool b)
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
