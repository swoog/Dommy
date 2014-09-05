//-----------------------------------------------------------------------
// <copyright file="SpeechListener.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Dommy.Business.Scenarios;
    using Dommy.Business.Tools;

    using JetBrains.Annotations;

    using Ninject.Extensions.Logging;

    /// <summary>
    /// Speech listener.
    /// </summary>
    public class SpeechListener : IListener
    {
        /// <summary>
        /// Gets logger error.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Object to lock.
        /// </summary>
        private readonly object obj = new object();

        /// <summary>
        /// Confidence target.
        /// </summary>
        private readonly double confidence;

        /// <summary>
        /// Dictionary of all sentences and scenario to execute.
        /// </summary>
        private readonly Dictionary<string, SpeechScenario> dicoScenario = new Dictionary<string, SpeechScenario>();

        /// <summary>
        /// Speech to text to use for speech listener.
        /// </summary>
        private readonly IList<ISpeechToText> speechToTexts;

        /// <summary>
        /// Gets actions logger.
        /// </summary>
        private readonly IList<IActionLogger> actionLoggers;

        /// <summary>
        /// Gets speech logger.
        /// </summary>
        private readonly ISpeechLogger speechLogger;

        /// <summary>
        /// Context grammar.
        /// </summary>
        private List<GrammarData> contextGrammar;

        /// <summary>
        /// Context functions.
        /// </summary>
        private Dictionary<string, Action<ISentence>> contextFunction;

        /// <summary>
        /// Action is in progress.
        /// </summary>
        private bool actionInProgress;

        /// <summary>
        /// Attached engine.
        /// </summary>
        private Engine engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeechListener"/> class.
        /// </summary>
        /// <param name="speechLogger">Speech logger.</param>
        /// <param name="logger">Error logger.</param>
        /// <param name="speechToTexts">Speech to text used.</param>
        /// <param name="actionLoggers">Actions loggers</param>
        /// <param name="confidence">Target confidence.</param>
        public SpeechListener(
                        ISpeechLogger speechLogger,
                        ILogger logger,
                        IList<ISpeechToText> speechToTexts,
                        IList<IActionLogger> actionLoggers,
                        double confidence)
        {
            this.actionLoggers = actionLoggers;
            this.speechLogger = speechLogger;
            this.speechToTexts = speechToTexts;
            this.logger = logger;
            this.confidence = confidence;
        }

        /// <summary>
        /// Gets or sets sentence log file.
        /// </summary>
        [UsedImplicitly]
        public string SentenceLogFile { get; set; }

        /// <summary>
        /// Initialize the listener.
        /// </summary>
        /// <param name="currentEngine">Current engine.</param>
        public void Init(Engine currentEngine)
        {
            this.engine = currentEngine;
            foreach (var item in this.speechToTexts)
            {
                item.Init();
            }

            this.logger.Info("Speech recognition intialized.");
        }

        /// <summary>
        /// Start listener.
        /// </summary>
        public void Start()
        {
            foreach (var item in this.speechToTexts)
            {
                if (item.IsActive)
                {
                    item.Start(this.SpeechRecognized);
                }
            }

            this.logger.Info("Wait for audio stream.");
        }

        /// <summary>
        /// Create grammar.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="sentences">Sentences triggers.</param>
        /// <param name="prefixName">Add prefix.</param>
        /// <returns>Grammar data.</returns>
        public GrammarData CreateGrammar(Action<string> action, IList<string> sentences, bool prefixName = false)
        {
            Contract.Requires(sentences != null);

            if (sentences.Count > 0)
            {
                var data = new GrammarData();

                if (prefixName)
                {
                    var dommy = new GrammarChoices();
                    dommy.Add(this.engine.Name);
                    data.Append(dommy);
                }

                var c = new GrammarChoices();
                foreach (var sentence in sentences)
                {
                    var text = sentence.Trim();

                    c.Add(text);

                    if (action != null)
                    {
                        action(text);
                    }
                }

                data.Append(c);

                return data;
            }

            return null;
        }

        /// <summary>
        /// Logs all the sentences authorized to recognized.
        /// </summary>
        public void Logs()
        {
            if (!string.IsNullOrEmpty(this.SentenceLogFile))
            {
                var sentences = this.dicoScenario.Keys.OrderBy(s => s);

                if (File.Exists(this.SentenceLogFile))
                {
                    File.Delete(this.SentenceLogFile);
                }

                using (var file = File.OpenWrite(this.SentenceLogFile))
                {
                    var writer = new StreamWriter(file);

                    foreach (var s in sentences)
                    {
                        writer.WriteLine(s);
                    }
                }
            }
        }

        /// <summary>
        /// Stop listener.
        /// </summary>
        public void Stop()
        {
            foreach (var item in this.speechToTexts)
            {
                item.Stop();
            }
        }

        /// <summary>
        /// Call a precision.
        /// </summary>
        /// <param name="sentenceActions">Sentences and actions to execute.</param>
        /// <param name="speech">Question speech.</param>
        internal void Precision(IEnumerable<SentenceAction> sentenceActions, string speech)
        {
            Contract.Requires(sentenceActions != null);

            // If a context grammar is loaded.
            this.UnloadContextGrammar();

            // Load all grammar.
            this.contextGrammar = new List<GrammarData>();
            this.contextFunction = new Dictionary<string, Action<ISentence>>();
            foreach (var item in sentenceActions)
            {
                var key = item.UniqueKey;

                if (string.IsNullOrEmpty(key))
                {
                    key = Guid.NewGuid().ToString();
                }

                SentenceAction item1 = item;
                var grammarInfo = DommyCache.Get(
                    string.Format(CultureInfo.InvariantCulture, "Grammar {0}", key),
                    TimeSpan.FromDays(30),
                    () =>
                    {
                        var sentences = new List<string>();
                        var g = this.CreateGrammar(sentences.Add, item1.Sentences);
                        return g;
                    });

                if (grammarInfo != null)
                {
                    this.contextGrammar.Add(grammarInfo);
                    foreach (var speechToText in this.speechToTexts)
                    {
                        speechToText.LoadGrammar(grammarInfo);
                    }

                    foreach (var s in item.Sentences)
                    {
                        if (!this.contextFunction.ContainsKey(s))
                        {
                            this.contextFunction.Add(s, item.Action);
                        }
                    }
                }
            }

            // Say
            this.speechLogger.Say(Actor.Dommy, speech);
        }

        /// <summary>
        /// Subscribe a trigger scenario to this speech listener.
        /// </summary>
        /// <param name="speechTrigger">Speech trigger.</param>
        /// <param name="scenario">Scenario to execute.</param>
        internal void Subscribe(Triggers.SpeechTrigger speechTrigger, IScenario scenario)
        {
            Contract.Requires(speechTrigger != null);

            var sentences = new List<string>();
            foreach (var s in speechTrigger.Speech)
            {
                var sentence = speechTrigger.WithPrefix ? string.Format(CultureInfo.InvariantCulture, "{0} {1}", this.engine.Name, s) : s;

                sentences.Add(sentence);
                this.dicoScenario.Add(sentence, new SpeechScenario { Scenario = scenario, Confidence = speechTrigger.Confidence });
            }

            var g = this.CreateGrammar(s => { }, sentences.ToArray());
            foreach (var item in this.speechToTexts)
            {
                item.LoadGrammar(g);
            }
        }

        /// <summary>
        /// Create grammar data from actions and sentences.
        /// </summary>
        private void UnloadContextGrammar()
        {
            if (this.contextGrammar != null)
            {
                foreach (var grammar in this.contextGrammar)
                {
                    this.logger.Debug("Unload context grammar {0}", grammar.ToString());
                    foreach (var item in this.speechToTexts)
                    {
                        item.UnloadGrammar(grammar);
                    }
                }

                this.contextGrammar = null;
                this.contextFunction = null;
            }
        }

        /// <summary>
        /// Speech recognized.
        /// </summary>
        /// <param name="sentence">Sentence recognized.</param>
        private void SpeechRecognized(ISentence sentence)
        {
            try
            {
                if (this.speechLogger.IgnoreRecognition)
                {
                    this.logger.Debug("Ignore : {0}", sentence.Text);
                    return;
                }

                lock (this.obj)
                {
                    if (this.actionInProgress)
                    {
                        return;
                    }

                    this.actionInProgress = true;
                }

                var words = sentence.Words;
                var wordsConfidence = sentence.WordsConfidence;

                if (this.dicoScenario.ContainsKey(sentence.Text))
                {
                    var s = this.dicoScenario[sentence.Text];
                    var currentConfidence = this.confidence;
                    if (s.Confidence.HasValue)
                    {
                        currentConfidence = s.Confidence.Value;
                    }

                    if (sentence.Confidence > currentConfidence)
                    {
                        if (words.First() == this.engine.Name)
                        {
                            var nameConfidence = wordsConfidence.First();
                            if (nameConfidence < this.confidence)
                            {
                                this.logger.Debug("Name confidence {0} ignored.", nameConfidence);
                                this.logger.Debug("Sentence : {0}.", sentence.Text);
                                return;
                            }
                            else
                            {
                                this.logger.Debug("Name confidence {0}.", nameConfidence);
                            }
                        }

                        this.UnloadContextGrammar();
                        this.logger.Debug("Confidence {0}, Cible {1}", sentence.Confidence, currentConfidence);
                        this.speechLogger.Say(Actor.Me, sentence.Text);

                        foreach (var l in this.actionLoggers)
                        {
                            l.ExecutionOf(s.Scenario);
                        }

                        s.Scenario.RunAsync();
                    }
                    else
                    {
                        this.logger.Info("Sentence ignored : {0}", sentence.Text);
                        this.logger.Debug("First word confidence {0}, Confidence {1}, Cible {2}", sentence.WordsConfidence.First(), sentence.Confidence, currentConfidence);
                    }

                    return;
                }

                if (sentence.Confidence > this.confidence)
                {
                    this.logger.Debug("Confidence {0}, Cible {1}", sentence.Confidence, this.confidence);

                    this.UnloadContextGrammar();

                    if (this.contextFunction != null && this.contextFunction.ContainsKey(sentence.Text))
                    {
                        var actionSentence = this.contextFunction[sentence.Text];

                        this.logger.Debug("Precision for {0}", sentence.Text);
                        this.SpeechRecognition(sentence, actionSentence);
                    }
                }
                else
                {
                    this.logger.Info("Text {0} Confidence {1}, Cible {2}", sentence.Text, sentence.Confidence, this.confidence);
                }
            }
            catch (Exception ex)
            {
                this.engine.SayError(ex);
            }
            finally
            {
                this.actionInProgress = false;
            }
        }

        /// <summary>
        /// Execute a recognition of sentence.
        /// </summary>
        /// <param name="sentence">Sentence recognized.</param>
        /// <param name="exec">Action to execute.</param>
        private void SpeechRecognition(ISentence sentence, Action<ISentence> exec)
        {
            Contract.Requires(sentence != null);
            Contract.Requires(exec != null);

            this.speechLogger.Say(Actor.Me, sentence.Text);

            exec(sentence);
        }

        /// <summary>
        /// Speech scenario to execute.
        /// </summary>
        private class SpeechScenario
        {
            /// <summary>
            /// Gets or sets scenario to execute.
            /// </summary>
            /// <returns></returns>
            public IScenario Scenario { get; set; }

            /// <summary>
            /// Gets or sets specific confidence to this scenario.
            /// </summary>
            /// <returns></returns>
            public double? Confidence { get; set; }
        }
    }
}
