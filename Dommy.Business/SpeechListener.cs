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
    using Ninject.Extensions.Logging;

    /// <summary>
    /// Speech listener.
    /// </summary>
    public class SpeechListener : IListener
    {
        /// <summary>
        /// Context grammar.
        /// </summary>
        private List<GrammarData> contextGrammar = null;

        /// <summary>
        /// Context functions.
        /// </summary>
        private Dictionary<string, Action<ISentence>> contextFunction = null;

        /// <summary>
        /// Action is in progress.
        /// </summary>
        private bool actionInProgress = false;

        /// <summary>
        /// Object to lock.
        /// </summary>
        private object obj = new object();

        /// <summary>
        /// Confidence target.
        /// </summary>
        private double confidence;

        /// <summary>
        /// Dictionary of all sentences and scenario to execute.
        /// </summary>
        private Dictionary<string, SpeechInfo> dicoScenario = new Dictionary<string, SpeechInfo>();

        /// <summary>
        /// Speech to text to use for speech listener.
        /// </summary>
        private ISpeechToText speechToText;

        /// <summary>
        /// Attached engine.
        /// </summary>
        private Engine engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeechListener"/> class.
        /// </summary>
        /// <param name="speechLogger">Speech logger.</param>
        /// <param name="logger">Error logger.</param>
        /// <param name="speechToText">Speech to text used.</param>
        /// <param name="actionLoggers">Actions loggers</param>
        /// <param name="confidence">Target confidence.</param>
        public SpeechListener(
                        ISpeechLogger speechLogger,
                        ILogger logger,
                        ISpeechToText speechToText,
                        IList<IActionLogger> actionLoggers,
                        double confidence)
        {
            this.ActionLoggers = actionLoggers;
            this.SpeechLogger = speechLogger;
            this.speechToText = speechToText;
            this.Logger = logger;
            this.confidence = confidence;
        }

        /// <summary>
        /// Gets or sets sentence log file.
        /// </summary>
        public string SentenceLogFile { get; set; }

        /// <summary>
        /// Gets actions logger.
        /// </summary>
        public IList<IActionLogger> ActionLoggers { get; private set; }

        /// <summary>
        /// Gets logger error.
        /// </summary>
        public ILogger Logger { get; private set; }

        /// <summary>
        /// Gets speech logger.
        /// </summary>
        public ISpeechLogger SpeechLogger { get; private set; }

        /// <summary>
        /// Initialize the listener.
        /// </summary>
        /// <param name="currentEngine">Current engine.</param>
        public void Init(Engine currentEngine)
        {
            this.engine = currentEngine;
            this.speechToText.Init();
            this.Logger.Info("Speech recognition intialized.");
        }

        /// <summary>
        /// Start listener.
        /// </summary>
        public void Start()
        {
            this.speechToText.Start(this.SpeechRecognized);
            this.Logger.Info("Wait for audio stream.");
        }

        /// <summary>
        /// Create a grammar data.
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
        /// Stop listener.
        /// </summary>
        public void Stop()
        {
            this.speechToText.Stop();
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
        /// Call a precision.
        /// </summary>
        /// <param name="sentenceActions">Sentences and actions to execute.</param>
        /// <param name="speech">Question speech.</param>
        internal void Precision(IList<SentenceAction> sentenceActions, string speech)
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

                var grammarInfo = DommyCache.Get<GrammarData>(
                    string.Format(CultureInfo.InvariantCulture, "Grammar {0}", key),
                    TimeSpan.FromDays(30),
                    () =>
                    {
                        var sentences = new List<string>();
                        var g = this.CreateGrammar(s => sentences.Add(s), item.Sentences);
                        return g;
                    });

                if (grammarInfo != null)
                {
                    this.contextGrammar.Add(grammarInfo);
                    this.speechToText.LoadGrammar(grammarInfo);

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
            this.SpeechLogger.Say(Actor.Dommy, speech);
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
                string sentence;

                if (speechTrigger.WithPrefix)
                {
                    sentence = string.Format(CultureInfo.InvariantCulture, "{0} {1}", this.engine.Name, s);
                }
                else
                {
                    sentence = s;
                }

                sentences.Add(sentence);
                this.dicoScenario.Add(sentence, new SpeechInfo { Scenario = scenario, Confidence = speechTrigger.Confidence });
            }

            var g = this.CreateGrammar(s => { }, sentences.ToArray());
            this.speechToText.LoadGrammar(g);
        }

        /// <summary>
        /// Unload context grammar.
        /// TODO : Timer unload 
        /// </summary>
        private void UnloadContextGrammar()
        {
            if (this.contextGrammar != null)
            {
                foreach (var grammar in this.contextGrammar)
                {
                    this.Logger.Debug("Unload context grammar {0}", grammar.ToString());
                    this.speechToText.UnloadGrammar(grammar);
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
                if (this.SpeechLogger.IgnoreRecognition)
                {
                    this.Logger.Debug("Ignore : {0}", sentence.Text);
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
                                this.Logger.Debug("Name confidence {0} ignored.", nameConfidence);
                                this.Logger.Debug("Sentence : {0}.", sentence.Text);
                                return;
                            }
                            else
                            {
                                this.Logger.Debug("Name confidence {0}.", nameConfidence);
                            }
                        }

                        this.UnloadContextGrammar();
                        this.Logger.Debug("Confidence {0}, Cible {1}", sentence.Confidence, currentConfidence);
                        this.SpeechLogger.Say(Actor.Me, sentence.Text);

                        foreach (var l in this.ActionLoggers)
                        {
                            l.ExecutionOf(s.Scenario);
                        }

                        s.Scenario.RunAsync();
                    }
                    else
                    {
                        this.Logger.Info("Sentence ignored : {0}", sentence.Text);
                        this.Logger.Debug("Confidence {0}, Cible {1}", sentence.Confidence, currentConfidence);
                    }

                    return;
                }

                if (sentence.Confidence > this.confidence)
                {
                    this.Logger.Debug("Confidence {0}, Cible {1}", sentence.Confidence, this.confidence);

                    if (this.contextFunction != null && this.contextFunction.ContainsKey(sentence.Text))
                    {
                        var actionSentence = this.contextFunction[sentence.Text];

                        this.UnloadContextGrammar();

                        this.Logger.Debug("Precision for {0}", sentence.Text);
                        this.SpeechRecognition(sentence, actionSentence);
                        return;
                    }

                    if (words.First() == this.engine.Name)
                    {
                        var nameConfidence = wordsConfidence.First();
                        if (nameConfidence < this.confidence)
                        {
                            this.Logger.Debug("Name confidence {0} ignored.", nameConfidence);
                            this.Logger.Debug("Sentence : {0}.", sentence.Text);
                            return;
                        }
                        else
                        {
                            this.Logger.Debug("Name confidence {0}.", nameConfidence);
                        }

                        words = words.Skip(1).ToArray();
                        wordsConfidence = wordsConfidence.Skip(1).ToArray();
                    }

                    this.UnloadContextGrammar();
                }
                else
                {
                    this.Logger.Info("Text {0} Confidence {1}, Cible {2}", sentence.Text, sentence.Confidence, this.confidence);
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

            this.SpeechLogger.Say(Actor.Me, sentence.Text);

            exec(sentence);
        }

        /// <summary>
        /// Speech scenario to execute.
        /// </summary>
        private class SpeechInfo
        {
            /// <summary>
            /// Gets or sets scenario.
            /// </summary>
            public IScenario Scenario { get; set; }

            /// <summary>
            /// Gets or sets confidence.
            /// </summary>
            public double? Confidence { get; set; }
        }
    }
}
