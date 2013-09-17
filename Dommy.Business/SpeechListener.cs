using Dommy.Business.Scenarios;
using Dommy.Business.Tools;
using Microsoft.Speech.Recognition;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business
{
    public class SpeechListener : IListener
    {
        private class SpeechInfo
        {
            public IScenario Scenario { get; set; }

            public double? Confidence { get; set; }
        }

        public string SentenceLogFile { get; set; }

        private List<Grammar> contextGrammar = null;
        private Dictionary<string, Func<ISentence, IResult>> contextFunction = null;

        public IList<IActionLogger> ActionLoggers { get; private set; }

        private bool actionInProgress = false;

        private object obj = new object();

        private double confidenceCible;
        private double confidence;

        private Dictionary<string, SpeechInfo> dicoScenario = new Dictionary<string, SpeechInfo>();
        private ISpeechToText speechToText;

        private Engine engine;

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
            this.confidenceCible = confidence;
            this.confidence = this.confidenceCible;
        }

        public void Init(Engine engine)
        {
            this.engine = engine;
            this.speechToText.Init();
            this.Logger.Info("Speech recognition intialized.");
        }

        public void Start()
        {
            this.speechToText.Start(SpeechRecognized);
            this.Logger.Info("Wait for audio stream.");
        }

        /// <summary>
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

        public Grammar CreateGrammar(Action<string> action, IList<string> sentences, bool prefixName = false)
        {
            if (sentences.Count > 0)
            {
                GrammarBuilder gb = new GrammarBuilder();

                if (prefixName)
                {
                    Choices dommy = new Choices();
                    dommy.Add(this.engine.Name);
                    gb.Append(dommy);
                }

                Choices c = new Choices();
                foreach (var sentence in sentences)
                {
                    var text = sentence.Trim();

                    c.Add(text);

                    if (action != null)
                    {
                        action(text);
                    }
                }

                gb.Append(c);

                return new Grammar(gb);
            }

            return null;
        }

        private void SpeechRecognized(ISentence sentence)
        {
            try
            {
                if (this.SpeechLogger.IgnoreRecognition)
                {
                    this.Logger.Debug("Ignore : {0}", sentence.Text);
                    return;
                }

                lock (obj)
                {
                    if (actionInProgress)
                    {
                        return;
                    }

                    actionInProgress = true;
                }
                    var words = sentence.Words;
                    var wordsConfidence = sentence.WordsConfidence;

                if (this.dicoScenario.ContainsKey(sentence.Text))
                {
                    var s = this.dicoScenario[sentence.Text];
                    var confidence = this.confidence;
                    if (s.Confidence.HasValue)
                    {
                        confidence = s.Confidence.Value;
                    }
                    if (sentence.Confidence > confidence)
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

                        UnloadContextGrammar();
                        this.Logger.Debug("Confidence {0}, Cible {1}", sentence.Confidence, confidence);
                        this.SpeechLogger.Say(Actor.Me, sentence.Text);

                        foreach (var l in this.ActionLoggers)
                        {
                            l.ExecutionOf(s.Scenario);
                        }

                        s.Scenario.Run();
                    }
                    else
                    {
                        this.Logger.Info("Sentence ignored : {0}", sentence.Text);
                        this.Logger.Debug("Confidence {0}, Cible {1}", sentence.Confidence, confidence);
                    }

                    return;
                }

                if (sentence.Confidence > this.confidence)
                {
                    this.Logger.Debug("Confidence {0}, Cible {1}", sentence.Confidence, this.confidence);

                    if (this.contextFunction != null && this.contextFunction.ContainsKey(sentence.Text))
                    {
                        var actionSentence = this.contextFunction[sentence.Text];

                        UnloadContextGrammar();

                        this.Logger.Debug("Precision for {0}", sentence.Text);
                        SpeechRecognition(sentence, actionSentence);
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

                    UnloadContextGrammar();
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
                actionInProgress = false;
            }
        }

        private void SpeechRecognition(ISentence sentence, Func<ISentence, IResult> exec)
        {
            this.SpeechLogger.Say(Actor.Me, sentence.Text);

            IResult result = exec(sentence);

            this.engine.RunResult(result);
        }

        public ILogger Logger { get; set; }

        public ISpeechLogger SpeechLogger { get; set; }

        internal void Precision(IList<Result.PrecisionResult.SentenceAction> sentenceActions, string speech)
        {
            // If a context grammar is loaded.
            this.UnloadContextGrammar();

            // Load all grammar.
            this.contextGrammar = new List<Grammar>();
            this.contextFunction = new Dictionary<string, Func<ISentence, IResult>>();
            foreach (var item in sentenceActions)
            {
                var key = item.UniqueKey;

                if (String.IsNullOrEmpty(key))
                {
                    key = Guid.NewGuid().ToString();
                }

                var grammarInfo = Cache.Get<GrammarInfo>(String.Format("Grammar {0}", key), TimeSpan.FromDays(30), () =>
                {
                    var sentences = new List<string>();
                    var g = this.CreateGrammar(s => sentences.Add(s), item.Sentences);
                    return new GrammarInfo
                    {
                        Grammar = g,
                        Sentences = sentences,
                    };
                });

                if (grammarInfo.Grammar != null)
                {
                    this.contextGrammar.Add(grammarInfo.Grammar);
                    this.speechToText.LoadGrammar(grammarInfo.Grammar);

                    foreach (var s in grammarInfo.Sentences)
                    {
                        if (!contextFunction.ContainsKey(s))
                        {
                            contextFunction.Add(s, item.Action);
                        }
                    }
                }
            }

            // Say
            this.SpeechLogger.Say(Actor.Dommy, speech);
        }

        private class GrammarInfo
        {
            public Grammar Grammar { get; set; }

            public IList<string> Sentences { get; set; }
        }

        public void Stop()
        {
            this.speechToText.Stop();
        }

        internal void Subscribe(Triggers.SpeechTrigger speechTrigger, IScenario scenario)
        {
            var sentences = new List<string>();
            foreach (var s in speechTrigger.Speech)
            {
                string sentence;

                if (speechTrigger.WithPrefix)
                {
                    sentence = String.Format("{0} {1}", this.engine.Name, s);
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

        public void Logs()
        {
            if (!String.IsNullOrEmpty(this.SentenceLogFile))
            {
                var sentences = this.dicoScenario.Keys.OrderBy(s => s);

                if (File.Exists(this.SentenceLogFile))
                {
                    File.Delete(this.SentenceLogFile);
                }

                using (var file = File.OpenWrite(this.SentenceLogFile))
                using (var writer = new StreamWriter(file))
                {
                    foreach (var s in sentences)
                    {
                        writer.WriteLine(s);
                    }

                    writer.Close();
                    file.Close();
                }
            }
        }
    }
}
